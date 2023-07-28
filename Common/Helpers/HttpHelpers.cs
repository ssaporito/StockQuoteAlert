namespace Common.Helpers.Http
{
    public static class HttpHelpers
    {
        public static string BuildQuery(string baseUrl, List<KeyValuePair<string, string>> queryParams)
        {
            string separator = queryParams.Any() ? "?" : "";
            string result = baseUrl + separator + string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
            return result;
        }
    }
}
