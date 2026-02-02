namespace HealthApp.Api.Middleware
{
    public class HttpHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeTask(HttpContext context)
        {
            context.Response.Headers["X-App-Name"] = "HealthApp";
            context.Response.Headers["Cache-Control"] = "no-store";

            await _next(context);

        }
        
    }
}
