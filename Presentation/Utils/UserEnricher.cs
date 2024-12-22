using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Http;

namespace Presentation.Utils
{
    public class UserEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var username = httpContext.User.Identity.Name;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Username", username));
            }
            else
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Username", "Anonymous"));
            }
        }
    }

}
