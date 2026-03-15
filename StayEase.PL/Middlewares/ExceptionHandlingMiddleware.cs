using System.Text.Json;

namespace StayEase.PL.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;  // بقلي شغال باي بيئه
        private readonly ILogger<ExceptionHandlingMiddleware> _logger; // لوجر عشان يسجل الخطأ

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IWebHostEnvironment environment,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception occurred while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                object response;

                if (_environment.IsDevelopment())
                {
                    response = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = ex.Message,
                        errorType = ex.GetType().Name,
                        stackTrace = ex.StackTrace
                    };
                }
                else
                {
                    response = new
                    {
                        statusCode = StatusCodes.Status500InternalServerError,
                        message = "Internal Server Error"
                    };
                }

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
