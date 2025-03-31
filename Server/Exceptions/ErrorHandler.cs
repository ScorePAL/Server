using System.Net;
using System.Text.Json;

namespace ScorePALServer.Exceptions;

public class ErrorHandler
{
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = 500;
            string message = "Undefined error";
            switch (exception)
            {
                case ScorePalException exScorePal:
                    statusCode = exScorePal.Code;
                    message = exScorePal.Message;
                    break;

                default: // Gestion des exceptions générales
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Une erreur interne s'est produite.";
                    break;

            }

            var response = new
            {
                status = statusCode,
                message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
}