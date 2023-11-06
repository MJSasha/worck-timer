namespace WorkTimer.Web.Definitons
{
    public static class Pages
    {
        public static class Login
        {
            private static string pageUrl = "/login";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }

        public static class Logout
        {
            private static string pageUrl = "/logout";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }

        public static class Statistic
        {
            private static string pageUrl = "/statistic";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }
    }
}