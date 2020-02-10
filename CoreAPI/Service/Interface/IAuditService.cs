using CoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Service.Interface
{
    public interface IAuditService
    {
        Task<IEnumerable<Audit>> GetAudits();
    }
}
