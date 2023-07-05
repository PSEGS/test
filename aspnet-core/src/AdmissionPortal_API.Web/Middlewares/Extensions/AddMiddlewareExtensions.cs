using Microsoft.AspNetCore.Builder;

namespace AdmissionPortal_API.Web.Middlewares.Extensions
{
    public static class AddMiddlewareExtensions
    {
        public static void UseAddMiddlewareExtensions(this IApplicationBuilder app)
        {
            app.UseMiddleware<AppMiddleware>();
        }
    }
}
