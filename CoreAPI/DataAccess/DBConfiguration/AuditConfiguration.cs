using CoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.DataAccess.DBConfiguration
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder?.ToTable("Audit");
            builder?.HasKey(a => a.Id);
            builder?.Property(a => a.Id).UseIdentityColumn(1, 1);

            builder.Property(a => a.Name).HasMaxLength(255).IsRequired(true);
            builder.Property(a => a.AuditDate).IsRequired(true).HasDefaultValueSql("GETDATE()");
        }
    }
}
