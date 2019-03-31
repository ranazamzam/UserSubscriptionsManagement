using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Infrastructure.EntityConfigurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasData(new User
            {
                Id = 1,
                FirstName = "test1first",
                LastName = "test1last",
                Email = "test1@gmail.com"
            },
            new User
            {
                Id = 2,
                FirstName = "test2first",
                LastName = "test2last",
                Email = "test2@gmail.com"
            });
        }
    }
}
