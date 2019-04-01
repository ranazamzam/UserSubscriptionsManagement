using Autofac;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Infrastructure;
using UserSubscriptionsManagement.Infrastructure.UnitOfWorks;
using UserSubscriptionsManagement.Services;
using UserSubscriptionsManagement.Services.Services;

namespace UserSubscriptionsManagement.WCFService
{
    public class DIModule
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            // register services

            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>();

            // register repositories and UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<ApplicationDbContext>();
            builder.RegisterType<MappingProfile>();

            //builder.Register(c =>
            //{
            //    //var config = c.Resolve<IConfiguration>();

            //    var opt = new DbContextOptionsBuilder<MyContext>();
            //    opt.UseSqlServer(config.GetSection("ConnectionStrings:MyConnection:ConnectionString").Value);

            //    return new MyContext(opt.Options);
            //}).Asse().InstancePerLifetimeScope();

            // build container
            return builder.Build();
        }
    }
}