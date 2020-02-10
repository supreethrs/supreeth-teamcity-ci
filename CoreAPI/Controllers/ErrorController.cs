using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : BaseController
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("{code}")]
        public IActionResult Error(int code)
        {
            HttpStatusCode parsedCode = (HttpStatusCode)code;

            string message = parsedCode.ToString();

            var contextFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                message = contextFeature.Error.ToString();
            }

            _logger.LogError($"{code} - {message}");

            return CustomResponse<string>(parsedCode, null, message);
            //return CustomResponse<string>(parsedCode, null, string.IsNullOrWhiteSpace(message) ? parsedCode.ToString() : message);
        }
    }
}