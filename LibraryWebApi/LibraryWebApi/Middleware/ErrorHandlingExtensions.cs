using Microsoft.AspNetCore.Builder;

namespace LibraryWebApi.Middleware
{
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}