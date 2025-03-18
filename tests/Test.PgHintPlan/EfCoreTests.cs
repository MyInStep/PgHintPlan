using System;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using PgHintPlan;
using PgHintPlan.EntityFrameworkCore;

namespace Test_PgHintPlan
{
    internal class ItemContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Item2> Items2 { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UsePgHintPlan();
            optionsBuilder.UseNpgsql();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("Items");
            modelBuilder.Entity<Product>().HasKey(x => x.Id).HasName("Products");
            modelBuilder.UsePgHintPlan();
        }
    }

    [PrimaryKey(nameof(Id))]
    [Index(nameof(Id), Name = "test_index")]
    public class Item
    {
        public Guid Id { get; set; }
    }

    [PrimaryKey(nameof(Id))]
    public class Item2
    {
        public Guid Id { get; set; }
    }

    [PrimaryKey(nameof(Id))]
    [Index(nameof(Id))]
    public class Product
    {
        public Guid Id { get; set; }
    }

    public class EfCoreTests
    {
        [Fact]
        public void InterceptorTest()
        {
            var ctx = new ItemContext();
            var cmd = ctx.Items
                .AsQueryable()
                .EnableHashAgg()
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
Set(enable_hashagg on)
*/".FixLineEndings());
        }

        [Fact]
        public void InterceptorSetFalseTest()
        {
            var ctx = new ItemContext();
            var cmd = ctx.Items
                .AsQueryable()
                .EnableHashAgg(false)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
Set(enable_hashagg off)
*/".FixLineEndings());
        }

        [Fact]
        public void InterceptorAllTest()
        {
            var ctx = new ItemContext();
            var cmd = ctx.Items
                .AsQueryable()
                .EnableHashAgg()
                .EnableHashJoin(false)
                .EnableIndexOnlyScan()
                .EnableIndexScan(false)
                .EnableMaterial()
                .EnableNestLoop(false)
                .EnablePartitionPruning()
                .EnablePartitionWiseAggregate(false)
                .EnablePartitionWiseJoin()
                .EnableSeqScan(false)
                .EnableSort()
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
Set(enable_hashagg on)
Set(enable_hashjoin off)
Set(enable_indexonlyscan on)
Set(enable_indexscan off)
Set(enable_material on)
Set(enable_nestloop off)
Set(enable_partition_pruning on)
Set(enable_partitionwise_aggregate off)
Set(enable_partitionwise_join on)
Set(enable_seqscan off)
Set(enable_sort on)
*/".FixLineEndings());
        }

        [Fact]
        public void InterceptorHashJoinTest()
        {
            var ctx = new ItemContext();

            var cmd = ctx.Products
                .Join(ctx.Items,
                (p) => p.Id,
                (i) => i.Id,
                (p, i) => new
                {
                    Pid = p.Id,
                    Id = i.Id
                })
                .HashJoin(ctx.Items.EntityType, ctx.Products.EntityType)
                .HashJoin(ctx.Products.EntityType, ctx.Items.EntityType)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
HashJoin(i p)
HashJoin(p i)
*/".FixLineEndings());
        }

        [Fact]
        public void InterceptorNestLoopTest()
        {
            var ctx = new ItemContext();

            var cmd = ctx.Products
                .Join(ctx.Items,
                (p) => p.Id,
                (i) => i.Id,
                (p, i) => new
                {
                    Pid = p.Id,
                    Id = i.Id
                })
                .NestLoop(ctx.Items.EntityType, ctx.Products.EntityType)
                .NestLoop(ctx.Products.EntityType, ctx.Items.EntityType)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
NestLoop(i p)
NestLoop(p i)
*/".FixLineEndings());
        }

        [Fact]
        public void InterceptorNoHashJoinTest()
        {
            var ctx = new ItemContext();

            var cmd = ctx.Products
                .Join(ctx.Items,
                (p) => p.Id,
                (i) => i.Id,
                (p, i) => new
                {
                    Pid = p.Id,
                    Id = i.Id
                })
                .NoHashJoin(ctx.Items.EntityType, ctx.Products.EntityType)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
NoHashJoin(i p)
*/".FixLineEndings());
        }

        [Fact]
        public void IndexScanTest()
        {
            var ctx = new ItemContext();
            var index = ctx.Items.EntityType
                .GetIndexes()
                .Where(i => i.Name == "test_index")
                .Single();

            var cmd = ctx.Items
                .AsQueryable()
                .IndexScan(ctx.Items.EntityType,index)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
IndexScan(i i_test_index)
*/".FixLineEndings());
        }
        [Fact]
        public void MultipleIndexScanTest()
        {
            var ctx = new ItemContext();
            var index = ctx.Items.EntityType.GetIndexes().FirstOrDefault();

            var cmd = ctx.Items
                .AsQueryable()
                .IndexScan(ctx.Items.EntityType,index,index)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
IndexScan(i i_test_index i_test_index)
*/".FixLineEndings());
        }
        [Fact]
        public void BitmapScanTest()
        {
            var ctx = new ItemContext();
            var index = ctx.Items.EntityType.GetIndexes().FirstOrDefault();

            var cmd = ctx.Items
                .AsQueryable()
                .BitmapScan(ctx.Items.EntityType,index)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
BitmapScan(i i_test_index)
*/".FixLineEndings());
        }
        [Fact]
        public void IndexScanRegexpTest()
        {
            var ctx = new ItemContext();
            var regex = "/((\\d3)(?:\\.|-))?(\\d3)(?:\\.|-)(\\d4)/";

            var cmd = ctx.Items
                .AsQueryable()
                .IndexScanRegexp(ctx.Items.EntityType,regex)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
IndexScanRegexp(i /((\d3)(?:\.|-))?(\d3)(?:\.|-)(\d4)/)
*/".FixLineEndings());
        }

        [Fact]
        public void SetRowsTest()
        {
            var ctx = new ItemContext();

            var cmd = ctx.Products
                .Join(ctx.Items,
                (p) => p.Id,
                (i) => i.Id,
                (p, i) => new
                {
                    Pid = p.Id,
                    Id = i.Id
                })
                .SetRows(ctx.Items.EntityType, ctx.Products.EntityType, RowsModifier.Add, 10)
                .SetRows(ctx.Products.EntityType, ctx.Items.EntityType, RowsModifier.Multiply, 100000000)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
Rows(i p +10)
Rows(p i *100000000)
*/".FixLineEndings());
        }

        [Fact]
        public void ParallelTest()
        {
            var ctx = new ItemContext();

            var cmd = ctx.Items
                .AsQueryable()
                .Parallel(ctx.Items.EntityType,5,EnforcementStrength.Soft)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
Parallel(i 5 soft)
*/".FixLineEndings());
        }

        [Fact]
        public void NoNestLoopTest()
        {
            var ctx = new ItemContext();
            var cmd = ctx.Items
                .Join(ctx.Items2,
                (i1) => i1.Id,
                (i2) => i2.Id,
                (i1, i2) => new
                {
                    Id1 = i1.Id,
                    Id2 = i2.Id
                })
                .NoNestLoop(ctx.Items.EntityType, ctx.Items2.EntityType)
                .CreateDbCommand();
            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
NoNestLoop(i i0)
*/".FixLineEndings());
        }
    }

    public static class StringExtensions
    {
        public static string FixLineEndings(this string value)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return value.Replace("\r\n", "\n");
            }
            else
            {
                return value;
            }
        }
    }
}