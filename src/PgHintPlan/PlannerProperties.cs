namespace PgHintPlan
{
    internal static class PlannerProperties
    {
        public const string EnableHashAgg                = "enable_hashagg";
        public const string EnableHashJoin               = "enable_hashjoin";
        public const string EnableIndexOnlyScan          = "enable_indexonlyscan";
        public const string EnableIndexScan              = "enable_indexscan";
        public const string EnableMaterial               = "enable_material";
        public const string EnableNestLoop             = "enable_nestloop";
        public const string EnableSeqScan                = "enable_seqscan";
        public const string EnableSort                   = "enable_sort";
        public const string EnablePartitionPruning       = "enable_partition_pruning";
        public const string EnablePartitionWiseJoin      = "enable_partitionwise_join";
        public const string EnablePartitionWiseAggregate = "enable_partitionwise_aggregate";
    }
}

