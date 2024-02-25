using System.Text;

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

            public static string GetUrl(bool showForUser = false)
            {
                if (showForUser)
                {
                    return BuildPageUrl($"{pageUrl}/true");
                }
                return BuildPageUrl(pageUrl);
            }
        }

        public static class Users
        {
            private static string pageUrl = "/users";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }

        public static class Calendar
        {
            private static string pageUrl = "/calendar";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }

        public static class Reports
        {
            private static string pageUrl = "/reports";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }

        public static class Information
        {
            private static string pageUrl = "/information";

            public static string GetUrl()
            {
                return pageUrl;
            }
        }

        public static class ReportsExportPage
        {
            private static string pageUrl = "/Reports";

            public static string GetUrl(string apiUrl, DateTime startDate, DateTime endDate)
            {
                return BuildPageUrl($"{apiUrl}{pageUrl}",
                    ("startDate", startDate),
                    ("endDate", endDate));
            }
        }

        private static string BuildPageUrl(string url, params (string name, object value)[] parameters)
        {
            StringBuilder stringBuilder = new(url);
            if (parameters != null && parameters.Length > 0)
            {
                stringBuilder.Append('?');
                stringBuilder.Append(string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.name)}={Uri.EscapeDataString(p.value.ToString())}")));
            }
            return stringBuilder.ToString();
        }
    }
}