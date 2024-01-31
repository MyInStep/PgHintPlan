using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using PgHintPlan.EntityFrameworkCore;

namespace Test.PgHintPlan
{
    public class ItemContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
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
    public class Item
    {
        public Guid Id { get; set; }
    }

    [PrimaryKey(nameof(Id))]
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
*/");
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
*/");
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
                .EnableNestedLoop(false)
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
*/");
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
                .HashJoin(ctx.Items, ctx.Products)
                .HashJoin(ctx.Products.EntityType, ctx.Items.EntityType)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
HashJoin(i p)
HashJoin(p i)
*/");
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
                .NoHashJoin(ctx.Items, ctx.Products)
                .CreateDbCommand();

            PgHintPlanInterceptor.ManipulateCommand(cmd);

            cmd.CommandText.Should().StartWith($@"/*+
NoHashJoin(i p)
*/");
        }
    }
}
