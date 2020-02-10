using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private const string DefaultErrorMessage = "Error Occurred";
        protected IActionResult CustomResponse<T>(HttpStatusCode code, T data = null, string message = "") where T : class
        {
            ApiResponse<T> response = new ApiResponse<T>();
            response.StatusCode = (int)code;

            switch (code)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.InternalServerError:
                    response.Success = false;
                    response.Message = string.IsNullOrWhiteSpace(message) ? DefaultErrorMessage : message;
                    break;
                default:
                    response.Success = true;
                    response.Result = data;
                    break;
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}