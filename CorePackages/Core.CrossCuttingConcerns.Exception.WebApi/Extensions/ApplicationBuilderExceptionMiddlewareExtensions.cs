using Microsoft.AspNetCore.Builder;
using Core.CrossCuttingConcerns.Exception.WebApi.Middleware;

namespace Core.CrossCuttingConcerns.Exception.WebApi.Extensions;

public static class ApplicationBuilderExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
