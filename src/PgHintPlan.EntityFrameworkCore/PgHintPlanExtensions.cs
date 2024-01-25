using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PgHintPlan;

namespace PgHintPlan.EntityFrameworkCore
{
    public static class PgHintPlanExtensions
    {
        public static IQueryable<T> EnableHasHagg<T>(this IQueryable<T> query, bool setter = true)
        {
            return query.TagWith(TagHelper.GenerateTag(PlannerProperties.EnableHasHagg, setter));
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
        


    }
}
