using Api_Intregacion.Middlewares;

namespace Api_Intregacion.Extensions
{
    public static class UseErrorHandlerMiddleware
    {
        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
