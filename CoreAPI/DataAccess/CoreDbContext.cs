using CoreAPI.DataAccess.DBConfiguration;
using CoreAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.DataAccess
{
    public class CoreDbContext : DbContext
    {
        private readonly IConfiguration _appConfiguration;
        //private readonly IHostingEnvironment _env;

        public CoreDbContext(IConfiguration configuration)
        {
            _appConfiguration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            SqlConnection connection = new SqlConnection() { ConnectionString = _appConfiguration["ConnectionStrings:CoreDatabase"] };

            //if (!_env.IsDevelopment())
            //{
            //    connection.AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
            //}

            optionsBuilder.UseSqlServer(connection);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder?.ApplyConfiguration(new AuditConfiguration());
            modelBuilder?.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<Audit> Audits { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
