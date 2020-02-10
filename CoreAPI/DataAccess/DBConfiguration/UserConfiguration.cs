using CoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.DataAccess.DBConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder?.HasKey(u => u.Id);
            builder?.Property(u => u.Id).UseIdentityColumn(1, 1);

            builder.Property(u => u.FirstName).HasMaxLength(255).IsRequired(true);
            builder.Property(u => u.LastName).HasMaxLength(255).IsRequired(false);

            builder.Property(u => u.Username).HasMaxLength(255).IsRequired(true);
            builder.Property(u => u.Password).HasMaxLength(255).IsRequired(true);

            builder.Ignore(u => u.Token).Ignore(u => u.FullName);
        }
    }
}
