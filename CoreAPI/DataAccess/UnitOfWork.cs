using CoreAPI.Models;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public CoreDbContext context { get; }
        private IDbContextTransaction _transaction { get; set; }
        private IConfiguration _configuration { get; }
        private readonly IHostEnvironment _env;

        public UnitOfWork(IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            context = context ?? CreateDBContext();
        }

        private CoreDbContext CreateDBContext()
        {
            return new CoreDbContext(_configuration);
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            }
        }

        public async void CommitTransaction()
        {
            await (_transaction?.CommitAsync()).ConfigureAwait(false);
        }

        public async void RollBackTransaction()
        {
            await (_transaction?.RollbackAsync()).ConfigureAwait(false);
        }

        public IRepository<Audit> AuditRepository => new GenericRepository<Audit, CoreDbContext>(context);
        public IRepository<User> UserRepository => new GenericRepository<User, CoreDbContext>(context);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }        
        #endregion


    }
}
