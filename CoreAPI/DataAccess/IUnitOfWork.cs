using CoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {

        CoreDbContext context { get; }
        Task<int> Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();

        IRepository<Audit> AuditRepository { get; }
        IRepository<User> UserRepository { get; }
    }
}
