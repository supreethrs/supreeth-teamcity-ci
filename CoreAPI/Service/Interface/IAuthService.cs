﻿using CoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Service.Interface
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
    }
}
