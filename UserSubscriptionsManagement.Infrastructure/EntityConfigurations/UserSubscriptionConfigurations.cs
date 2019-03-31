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
    public class UserSubscriptionConfigurations: IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.HasKey(e => new { e.UserId, e.SubscriptionId });

            builder.HasOne<User>(e => e.User)
                   .WithMany(e => e.Subscriptions)
                   .HasForeignKey(e => e.UserId);


            builder.HasOne<Subscription>(e => e.Subscription)
                   .WithMany(e => e.Users)
                   .HasForeignKey(e => e.SubscriptionId);
            
        }
    }
}
