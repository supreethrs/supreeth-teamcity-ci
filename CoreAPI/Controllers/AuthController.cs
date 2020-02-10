using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreAPI.Models;
using CoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest?.Username) || string.IsNullOrWhiteSpace(userRequest?.Password))
                return CustomResponse<string>(HttpStatusCode.BadRequest, null, "Username / password is blank");
            // return BadRequest(new { message = "Username / password is blank" });

            var user = await _authService.Authenticate(userRequest.Username, userRequest.Password).ConfigureAwait(false);

            if (user == null)
                return CustomResponse<string>(HttpStatusCode.BadRequest, null, "Incorrect Username / password");
            // return BadRequest(new { message = "Incorrect Username / password" });

            // return Ok(user);
            return CustomResponse(HttpStatusCode.OK, user);
        }
    }
}