using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace PgHintPlan.EntityFrameworkCore
{
    public static class PgHintPlanExtensions
    {
        public static ModelBuilder UsePgHintPlan(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_hint_plan");
            return modelBuilder;
        }

        public static DbContextOptionsBuilder UsePgHintPlan(this DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new PgHintPlanInterceptor());
            return optionsBuilder;
        }

        public static IQueryable<T> EnableHashAgg<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableHashAgg, setter));
        }

        public static IQueryable<T> EnableHashJoin<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableHashJoin, setter));
        }

        public static IQueryable<T> EnableIndexOnlyScan<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableIndexOnlyScan, setter));
        }

        public static IQueryable<T> EnableIndexScan<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableIndexScan, setter));
        }

        public static IQueryable<T> EnableMaterial<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableMaterial, setter));
        }

        public static IQueryable<T> EnableNestedLoop<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableNestedLoop, setter));
        }

        public static IQueryable<T> EnablePartitionPruning<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnablePartitionPruning, setter));
        }

        public static IQueryable<T> EnablePartitionWiseAggregate<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnablePartitionWiseAggregate, setter));
        }

        public static IQueryable<T> EnablePartitionWiseJoin<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnablePartitionWiseJoin, setter));
        }

        public static IQueryable<T> EnableSeqScan<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableSeqScan, setter));
        }

        public static IQueryable<T> EnableSort<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableSort, setter));
        }

        public static IQueryable<T> HashJoin<T>(this IQueryable<T> query, params IEntityType[] args)
             where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.HashJoin, args.Select(a => a.GetTableName()).ToArray()));
        }

        public static IQueryable<T> NoHashJoin<T>(this IQueryable<T> query, params IEntityType[] args)
             where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoHashJoin, args.Select(a => a.GetTableName()).ToArray()));
        }


        public static IQueryable<T> NestLoop<T>(this IQueryable<T> query, params IEntityType[] args)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NestLoop, args.Select(a => a.GetTableName()).ToArray()));
        }

        public static IQueryable<T> NoNestLoop<T>(this IQueryable<T> query, params IEntityType[] args)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoNestLoop, args.Select(a => a.GetTableName()).ToArray()));
        }
        /// <summary>
        /// Forces sequential scan on the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IQueryable<T> SeqScan<T>(this IQueryable<T> query, IEntityType entity)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.SeqScan, entity.GetTableName()));
        }

        /// <summary>
        /// Forces TID scan on the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IQueryable<T> TidScan<T>(this IQueryable<T> query, IEntityType entity)
           where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.TidScan, entity.GetTableName()));
        }

        /// <summary>
        /// Forces index scan on the table. Restricts to specified indexes if any.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="entity"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static IQueryable<T> IndexScan<T>(this IQueryable<T> query, IEntityType entity, params IIndex[] indexes)
             where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(query));
            ArgumentException.ThrowIfNullOrEmpty(nameof(entity));
            ArgumentException.ThrowIfNullOrEmpty(nameof(indexes));

            var args = new List<string>();

            args.Add(entity.GetTableName());

            foreach(var i in indexes)
            {
                args.Add($"{entity.GetTableName()}_{i.GetDatabaseName()}");

            }
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexScan, args.ToArray()));
        }
        /// <summary>
        /// Forces index-only scan on the table. Restricts to specified indexes if any.
        /// Index scan may be used if index-only scan is not available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="entity"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static IQueryable<T> IndexOnlyScan<T>(this IQueryable<T> query, IEntityType entity, params IIndex[] indexes)
            where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(query));
            ArgumentException.ThrowIfNullOrEmpty(nameof(entity));
            ArgumentException.ThrowIfNullOrEmpty(nameof(indexes));

            var args = new List<string>();

            args.Add(entity.GetTableName());

            foreach (var i in indexes)
            {
                args.Add($"{entity.GetTableName()}_{i.GetDatabaseName()}");

            }
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexOnlyScan, args.ToArray()));
        }
        /// <summary>
        /// Forces bitmap scan on the table. Restricts to specified indexes if any.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="entity"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static IQueryable<T> BitmapScan<T>(this IQueryable<T> query, IEntityType entity, params IIndex[] indexes)
          where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(query));
            ArgumentException.ThrowIfNullOrEmpty(nameof(entity));
            ArgumentException.ThrowIfNullOrEmpty(nameof(indexes));

            var args = new List<string>();

            args.Add(entity.GetTableName());

            foreach (var i in indexes)
            {
                args.Add($"{entity.GetTableName()}_{i.GetDatabaseName()}");

            }
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.BitmapScan, args.ToArray()));
        }

        public static IQueryable<T> IndexScanRegexp<T>(this IQueryable<T> query, IEntityType entity,string regex)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexScanRegexp, entity.GetTableName() , regex));
        }
        public static IQueryable<T> IndexOnlyScanRegexp<T>(this IQueryable<T> query, IEntityType entity, string regex)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexOnlyScanRegexp, entity.GetTableName(), regex));
        }
        public static IQueryable<T> BitmapScanRegexp<T>(this IQueryable<T> query, IEntityType entity, string regex)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexOnlyScanRegexp, entity.GetTableName(), regex));
        }
        public static IQueryable<T> NoSeqScan<T>(this IQueryable<T> query, IEntityType entity)
           where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoSeqScan, entity.GetTableName()));
        }
        public static IQueryable<T> NoTidScan<T>(this IQueryable<T> query, IEntityType entity)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoSeqScan, entity.GetTableName()));
        }
        public static IQueryable<T> NoIndexScan<T>(this IQueryable<T> query, IEntityType entity)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoIndexScan, entity.GetTableName()));
        }
        public static IQueryable<T> NoIndexOnlyScan<T>(this IQueryable<T> query, IEntityType entity)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoIndexOnlyScan, entity.GetTableName()));
        }
        public static IQueryable<T> NoBitmapScan<T>(this IQueryable<T> query, IEntityType entity)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoBitmapScan, entity.GetTableName()));
        }
        public static IQueryable<T> Leading<T>(this IQueryable<T> query, params IEntityType[] args)
           where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.Leading, args.Select(a => a.GetTableName()).ToArray()));
        }
        public static IQueryable<T> Memoize<T>(this IQueryable<T> query, params IEntityType[] args)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.Memoize, args.Select(a => a.GetTableName()).ToArray()));
        }
        public static IQueryable<T> NoMemoize<T>(this IQueryable<T> query, params IEntityType[] args)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoMemoize, args.Select(a => a.GetTableName()).ToArray()));
        }
        public static IQueryable<T> SetRows<T>(this IQueryable<T> query, IEntityType t1, IEntityType t2, RowsModifier modifier, int value)
          where T : class
        {
            string modifierString = modifier switch
            {
                RowsModifier.Set      => "#",
                RowsModifier.Add      => "+",
                RowsModifier.Subtract => "-",
                RowsModifier.Multiply => "*",
                _ => throw new ArgumentException($"Invalid modifier {modifier}", nameof(modifier))
            };

            modifierString += value;

            return query.TagWith(TagHelper.GenerateTag(MiscMethods.Rows, t1.GetTableName(), t2.GetTableName(), modifierString));
        }

    }
}
