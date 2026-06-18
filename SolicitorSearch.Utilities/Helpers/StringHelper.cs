using System.Net;

namespace SolicitorSearch.Utilities.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// removes html symbol codes from the input string
        /// </summary>
        public static string Normalise(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Decode HTML entities (e.g. &amp; &nbsp; &#39; etc.)
            var decoded = WebUtility.HtmlDecode(input);
            return decoded;
        }
    }
}
