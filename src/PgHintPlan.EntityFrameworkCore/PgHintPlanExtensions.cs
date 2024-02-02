using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PgHintPlan.EntityFrameworkCore
{
    /// <summary>
    /// pg_hint_plan extension methods.
    /// </summary>
    public static class PgHintPlanExtensions
    {
        /// <summary>
        /// Adds the pg_hint_plan extension to the model.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to enable pg_hint_plan on.</param>
        /// <returns></returns>
        public static ModelBuilder UsePgHintPlan(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_hint_plan");
            return modelBuilder;
        }
        /// <summary>
        /// Adds the PgHintPlanInterceptor.
        /// </summary>
        /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> to enable pg_hint_plan on.</param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UsePgHintPlan(this DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new PgHintPlanInterceptor());
            return optionsBuilder;
        }

        /// <summary>
        /// Enables or disables the query planner's use of hashed aggregation plan types.
        ///The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableHashAgg<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableHashAgg, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of hashed join plan types.
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableHashJoin<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableHashJoin, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of index-only-scan plan types.
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableIndexOnlyScan<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableIndexOnlyScan, enabled));
        }
        /// <summary>
        /// Enables or disables the query planner's use of index-scan plan types.
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableIndexScan<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableIndexScan, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of materialization
        /// It is impossible to suppress materialization entirely,
        /// but turning this variable off prevents the planner from inserting
        /// materialize nodes except in cases where it is required for correctness.
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableMaterial<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableMaterial, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of nested-loop join plans.
        /// It is impossible to suppress nested-loop joins entirely,
        /// but turning this variable off discourages the planner from using one if there are other methods available
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableNestLoop<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableNestLoop, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's
        /// ability to eliminate a partitioned
        /// table's partitions from query plans.
        /// This also controls the planner's
        /// ability to generate query plans which
        /// allow the query executor to remove (ignore)
        /// partitions during query execution
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnablePartitionPruning<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnablePartitionPruning, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of partitionwise grouping or aggregation,
        /// which allows grouping or aggregation on partitioned tables to be performed separately for each partition.
        /// If the GROUP BY clause does not include the partition keys, only partial aggregation can be performed on
        /// a per-partition basis, and finalization must be performed later.
        /// Because partitionwise grouping or aggregation can use significantly more CPU time and memory during planning,
        /// the default is off.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnablePartitionWiseAggregate<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnablePartitionWiseAggregate, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of partitionwise join,
        /// which allows a join between partitioned tables to be performed by joining the matching partitions.
        /// Partitionwise join currently applies only when the join conditions include all the partition keys,
        /// which must be of the same data type and have one-to-one matching sets of child partitions.
        /// Because partitionwise join planning can use significantly more CPU time and memory during planning,
        /// the default is off.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnablePartitionWiseJoin<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnablePartitionWiseJoin, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of sequential scan plan types.
        /// It is impossible to suppress sequential scans entirely,
        /// but turning this variable off discourages the planner from using one if there are other methods available.
        /// The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableSeqScan<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableSeqScan, enabled));
        }

        /// <summary>
        /// Enables or disables the query planner's use of explicit sort steps.
        /// It is impossible to suppress explicit sorts entirely,
        /// but turning this variable off discourages the planner from using
        /// one if there are other methods available. The default is on.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="enabled">Whether to enable the option. (defaults to true)</param>
        /// <returns></returns>
        public static IQueryable<T> EnableSort<T>(this IQueryable<T> query, bool enabled = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableSort, enabled));
        }

        /// <summary>
        /// Forces hash join for the joins on the tables specified.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> HashJoin<T>(this IQueryable<T> query, params IEntityType[] args)
             where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.HashJoin, args.Select(a => a.GetTableName()).ToArray()));
        }
        /// <summary>
        /// Forces to not do hash join for the joins on the tables specified.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> NoHashJoin<T>(this IQueryable<T> query, params IEntityType[] args)
             where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoHashJoin, args.Select(a => a.GetTableName()).ToArray()));
        }

        /// <summary>
        /// Forces nested loop for the joins on the tables specified.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> NestLoop<T>(this IQueryable<T> query, params IEntityType[] args)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NestLoop, args.Select(a => a.GetTableName()).ToArray()));
        }
        /// <summary>
        /// Forces to not do nested loop for the joins on the tables specified.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> NoNestLoop<T>(this IQueryable<T> query, params IEntityType[] args)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoNestLoop, args.Select(a => a.GetTableName()).ToArray()));
        }
        /// <summary>
        /// Forces sequential scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> SeqScan<T>(this IQueryable<T> query, IEntityType entityType)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.SeqScan, entityType.GetTableName()));
        }

        /// <summary>
        /// Forces TID scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> TidScan<T>(this IQueryable<T> query, IEntityType entityType)
           where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.TidScan, entityType.GetTableName()));
        }

        /// <summary>
        /// Forces index scan on the table. Restricts to specified indexes if any.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="indexes">An array of <see cref="IIndex"/> parameters representing indexes.</param>
        /// <returns></returns>
        public static IQueryable<T> IndexScan<T>(this IQueryable<T> query, IEntityType entityType, params IIndex[] indexes)
             where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(query));
            ArgumentException.ThrowIfNullOrEmpty(nameof(entityType));
            ArgumentException.ThrowIfNullOrEmpty(nameof(indexes));

            var args = new List<string>();

            args.Add(entityType.GetTableName());

            foreach(var i in indexes)
            {
                args.Add($"{entityType.GetTableName()}_{i.GetDatabaseName()}");

            }
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexScan, args.ToArray()));
        }

        /// <summary>
        /// Forces index-only scan on the table. Restricts to specified indexes if any.
        /// Index scan may be used if index-only scan is not available.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="indexes">An array of <see cref="IIndex"/> parameters representing indexes.</param>
        /// <returns></returns>
        public static IQueryable<T> IndexOnlyScan<T>(this IQueryable<T> query, IEntityType entityType, params IIndex[] indexes)
            where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(query));
            ArgumentException.ThrowIfNullOrEmpty(nameof(entityType));
            ArgumentException.ThrowIfNullOrEmpty(nameof(indexes));

            var args = new List<string>();

            args.Add(entityType.GetTableName());

            foreach (var i in indexes)
            {
                args.Add($"{entityType.GetTableName()}_{i.GetDatabaseName()}");

            }
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexOnlyScan, args.ToArray()));
        }

        /// <summary>
        /// Forces bitmap scan on the table. Restricts to specified indexes if any.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="indexes">An array of <see cref="IIndex"/> parameters representing indexes.</param>
        /// <returns></returns>
        public static IQueryable<T> BitmapScan<T>(this IQueryable<T> query, IEntityType entityType, params IIndex[] indexes)
          where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(query));
            ArgumentException.ThrowIfNullOrEmpty(nameof(entityType));
            ArgumentException.ThrowIfNullOrEmpty(nameof(indexes));

            var args = new List<string>();

            args.Add(entityType.GetTableName());

            foreach (var i in indexes)
            {
                args.Add($"{entityType.GetTableName()}_{i.GetDatabaseName()}");

            }
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.BitmapScan, args.ToArray()));
        }

        /// <summary>
        /// Forces index scan on the table. Restricts to indexes that matches the specified POSIX regular expression pattern.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static IQueryable<T> IndexScanRegexp<T>(this IQueryable<T> query, IEntityType entityType, string regex)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexScanRegexp, entityType.GetTableName() , regex));
        }

        /// <summary>
        /// Forces index-only scan on the table. Restricts to indexes that matches the specified POSIX regular expression pattern.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static IQueryable<T> IndexOnlyScanRegexp<T>(this IQueryable<T> query, IEntityType entityType, string regex)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexOnlyScanRegexp, entityType.GetTableName(), regex));
        }

        /// <summary>
        /// Forces bitmap scan on the table. Restricts to indexes that matches the specified POSIX regular expression pattern.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static IQueryable<T> BitmapScanRegexp<T>(this IQueryable<T> query, IEntityType entityType, string regex)
            where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.IndexOnlyScanRegexp, entityType.GetTableName(), regex));
        }

        /// <summary>
        /// Forces to not do sequential scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> NoSeqScan<T>(this IQueryable<T> query, IEntityType entityType)
           where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoSeqScan, entityType.GetTableName()));
        }

        /// <summary>
        /// Forces to not do TID scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> NoTidScan<T>(this IQueryable<T> query, IEntityType entityType)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoSeqScan, entityType.GetTableName()));
        }

        /// <summary>
        /// Forces to not do index scan and index-only scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> NoIndexScan<T>(this IQueryable<T> query, IEntityType entityType)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoIndexScan, entityType.GetTableName()));
        }

        /// <summary>
        ///Forces to not do index only scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> NoIndexOnlyScan<T>(this IQueryable<T> query, IEntityType entityType)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoIndexOnlyScan, entityType.GetTableName()));
        }

        /// <summary>
        /// Forces to not do bitmap scan on the table.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <returns></returns>
        public static IQueryable<T> NoBitmapScan<T>(this IQueryable<T> query, IEntityType entityType)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(ScanMethods.NoBitmapScan, entityType.GetTableName()));
        }

        /// <summary>
        ///Forces join order as specified.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> Leading<T>(this IQueryable<T> query, params IEntityType[] args)
           where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.Leading, args.Select(a => a.GetTableName()).ToArray()));
        }

        /// <summary>
        /// Allows the topmost join of a join among the specified tables to Memoize the inner result. Not enforced.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> Memoize<T>(this IQueryable<T> query, params IEntityType[] args)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.Memoize, args.Select(a => a.GetTableName()).ToArray()));
        }

        /// <summary>
        /// Inhibits the topmost join of a join among the specified tables from Memoizing the inner result.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="args">An array of <see cref="IEntityType"/> parameters representing tables.</param>
        /// <returns></returns>
        public static IQueryable<T> NoMemoize<T>(this IQueryable<T> query, params IEntityType[] args)
          where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoMemoize, args.Select(a => a.GetTableName()).ToArray()));
        }

        /// <summary>
        /// Corrects row number of a result of the joins on the tables specified.
        /// The available correction methods are absolute (#), addition (+), subtract (-) and multiplication (*).
        /// Should be a string that strtod() can understand.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="t1">The first table in the rows hint.</param>
        /// <param name="t2">The second table in the rows hint.</param>
        /// <param name="modifier">The <see cref="RowsModifier"/>.</param>
        /// <param name="value">The modifier value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Enforces or inhibits parallel execution of the specified table.
        /// <# of workers> is the desired number of parallel workers, where zero means inhibiting parallel execution.
        /// If the third parameter is soft (default), it just changes max_parallel_workers_per_gather and leaves everything
        /// else to the planner. Hard enforces the specified number of workers.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the <see cref="IQueryable"/>.</typeparam>
        /// <param name="query">A query to add the hint to.</param>
        /// <param name="entityType">The <see cref="IEntityType"/> representing the table.</param>
        /// <param name="value">The parallelism value.</param>
        /// <param name="strength">The <see cref="EnforcementStrength"/> of the parallelism.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IQueryable<T> Parallel<T>(this IQueryable<T> query, IEntityType entityType, int value, EnforcementStrength strength)
        where T : class
        {
            string strengthString = strength switch
            {
                EnforcementStrength.Soft => "soft",
                EnforcementStrength.Hard => "hard",
                _ => throw new ArgumentException($"Invalid modifier {strength}", nameof(strength))
            };

            return query.TagWith(TagHelper.GenerateTag(MiscMethods.Parallel, entityType.GetTableName(), value.ToString(), strengthString));
        }

    }
}
