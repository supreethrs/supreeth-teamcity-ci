﻿using System;
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
using Microsoft.Extensions.Logging;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Authorize]
    public class AuditController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IAuditService _auditService;
        public AuditController(ILogger<AuditController> logger, IAuditService auditService)
        {
            _logger = logger;
            _auditService = auditService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
                _logger.LogInformation("Start Service");
                var audits = await _auditService.GetAudits().ConfigureAwait(false);
                return CustomResponse(HttpStatusCode.OK, audits);
        }

        [HttpGet("Get1")]
        public async Task<IActionResult> Get1()
        {
            int i = 0;
            i = 1 / i;
            return CustomResponse(HttpStatusCode.OK, i.ToString());
        }
    }
}