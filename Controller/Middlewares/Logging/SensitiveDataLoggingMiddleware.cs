namespace ScorePALServerController.Middlewares.Logging;

// Enhanced middleware for sensitive data filtering
public class SensitiveDataLoggingMiddleware(RequestDelegate next, ILogger<SensitiveDataLoggingMiddleware> logger)
{
    // Easy to extend these patterns as your API evolves
    private static readonly string[] SensitiveEndpoints =
    [
        "/api/user/login", "/api/user/register", "/api/user/refresh-token", "/api/user/reset-password"

    ];

    private static readonly string[] SensitiveParameters =
    [
        "password", "token", "secret", "key", "credential"
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        var query = context.Request.QueryString.Value?.ToLower() ?? "";

        // Check if request contains sensitive endpoints or parameters
        var isSensitiveEndpoint = SensitiveEndpoints.Any(endpoint => path.Contains(endpoint));
        var hasSensitiveParams = SensitiveParameters.Any(param => query.Contains(param));

        if (isSensitiveEndpoint || hasSensitiveParams)
        {
            // Log basic request info without sensitive details
            logger.LogInformation("Request: {Method} {Path} - Body and sensitive headers omitted",
                context.Request.Method,
                context.Request.Path);
        }
        else
        {
            // Normal logging for non-sensitive endpoints
            logger.LogDebug("Request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }

        await next(context);
    }
}