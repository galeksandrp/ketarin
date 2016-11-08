namespace MyDownloader.Core
{
    internal class Settings
    {
        public static int MaxRetries { get; set; }

        public static double RetryDelay { get; set; }

        public static int MaxSegments { get; set; }

        public static double MinSegmentLeftToStartNewSegment { get; set; }

        public static long MinSegmentSize { get; set; }

        public static string ProxyAddress { get; set; }

        public static string ProxyUserName { get; set; }

        public static string ProxyPassword { get; set; }

        public static string ProxyDomain { get; set; }

        public static bool UseProxy { get; set; }

        public static bool ProxyByPassOnLocal { get; set; }

        public static int ProxyPort { get; set; }
    }
}