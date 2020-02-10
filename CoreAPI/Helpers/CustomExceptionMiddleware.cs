using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace CoreAPI.Helpers
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // await _next.Invoke(context).ConfigureAwait(false);
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static HttpStatusCode GetErrorCode(Exception e)
        {
            switch (e)
            {
                case ValidationException _:
                    return HttpStatusCode.BadRequest;
                case AuthenticationException _:
                    return HttpStatusCode.Forbidden;
                case NotImplementedException _:
                    return HttpStatusCode.NotImplemented;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            // var customException = exception as BaseCustomException;
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            var statusCode = (int)HttpStatusCode.InternalServerError;

            if (contextFeature != null)
            {
                statusCode = (int)GetErrorCode(contextFeature.Error);
            }
            
            var message = exception != null && !string.IsNullOrWhiteSpace(exception?.Message) ? exception.Message : "Unexpected error";

            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            _logger.LogError(exception, message);

            await response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse<string>
            {
                Message = message,
                Success = false,
                StatusCode = statusCode
            })).ConfigureAwait(false);
        }
    }
}
