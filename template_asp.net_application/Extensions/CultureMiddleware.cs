using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace template_asp.net_application.Extensions
{
    public sealed class CultureMiddleware
    {
        private static string DefaultCultureHeaderOrQueryName { get; } = "culture";
        private RequestDelegate Next { get; }
        private static Dictionary<string, string> _supportedCulture = new Dictionary<string, string>
        {
            { "en-Us", "en-Us" },
            { "ru-Ru", "ru-Ru" },
        };


        public CultureMiddleware(RequestDelegate next)
        {
            Next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var culture = context.Request.Headers[DefaultCultureHeaderOrQueryName].FirstOrDefault(); if (culture.IsNullOrEmpty())
                culture = context.Request.Query[DefaultCultureHeaderOrQueryName];
            if (_supportedCulture.TryGetValue(culture ?? string.Empty, out var cultureValue))
            {
                var cultureInfo = CultureInfo.GetCultureInfo(cultureValue); CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }
            await Next(context);
        }
    }
}
