using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Infrastructure.EntityConfigurations
{
    public class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(new Subscription
            {
                Id = Guid.NewGuid(),
                Name = "50 min deal",
                Price = 24.0,
                PriceIncVatAmount = 30,
                CallMinutes = 50
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                Name = "20 min deal",
                Price = 10.0,
                PriceIncVatAmount = 25,
                CallMinutes = 20
            });

        }
    }
}
