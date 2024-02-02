namespace PgHintPlan
{
    internal static class ScanMethods
    {
        public const string SeqScan   = "SeqScan";
        public const string TidScan   = "TidScan";
        public const string IndexScan = "IndexScan";
        public const string IndexOnlyScan = "IndexOnlyScan";
        public const string BitmapScan = "BitmapScan";
        public const string IndexScanRegexp = "IndexScanRegexp";
        public const string IndexOnlyScanRegexp = "IndexOnlyScanRegexp";
        public const string BitmapScanRegexp = "BitmapScanRegexp";
        public const string NoSeqScan = "NoSeqScan";
        public const string NoTidScan = "NoTidScan";
        public const string NoIndexScan = "NoIndexScan";
        public const string NoIndexOnlyScan = "NoIndexOnlyScan";
        public const string NoBitmapScan = "NoBitmapScan";
    }
}
