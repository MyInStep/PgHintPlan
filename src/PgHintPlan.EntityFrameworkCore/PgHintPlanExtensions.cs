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

        public static IQueryable<T> HashJoin<T>(this IQueryable<T> query, params string[] args)
             where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.HashJoin, args));
        }

        public static IQueryable<T> HashJoin<T>(this IQueryable<T> query, params IEntityType[] args)
             where T : class
        {
            return query.HashJoin(args.Select(a => a.GetTableName()).ToArray());
        }

        public static IQueryable<T> HashJoin<T>(this IQueryable<T> query, params object[] args)
             where T : class
        {
            var stringArgs = new List<string>();
            foreach (object o in args)
            {
                var entityTypeProperty = o.GetType().GetProperty("EntityType");
                IEntityType entityType = (IEntityType)entityTypeProperty.GetValue(o);

                stringArgs.Add(entityType.GetTableName());
            }
            return query.HashJoin(stringArgs.ToArray());
        }

        public static IQueryable<T> NoHashJoin<T>(this IQueryable<T> query, params string[] args)
             where T : class
        {
            return query.TagWith(TagHelper.GenerateTag(JoinMethods.NoHashJoin, args));
        }

        public static IQueryable<T> NoHashJoin<T>(this IQueryable<T> query, params IEntityType[] args)
             where T : class
        {
            return query.NoHashJoin(args.Select(a => a.GetTableName()).ToArray());
        }

        public static IQueryable<T> NoHashJoin<T>(this IQueryable<T> query, params object[] args)
             where T : class
        {
            var stringArgs = new List<string>();
            foreach (object o in args)
            {
                var entityTypeProperty = o.GetType().GetProperty("EntityType");
                IEntityType entityType = (IEntityType)entityTypeProperty.GetValue(o);

                stringArgs.Add(entityType.GetTableName());
            }
            return query.NoHashJoin(stringArgs.ToArray());
        }
    }
}
