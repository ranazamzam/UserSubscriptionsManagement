using Autofac;
using Autofac.Integration.Wcf;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using UserSubscriptionsManagement.Services;

namespace UserSubscriptionsManagement.WebAPI
{
    public class DIModule
    {
        public static IContainer BuildContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // register services

            builder.Register(c => new ChannelFactory<IUserService>(
                       new BasicHttpBinding(),
                       new EndpointAddress("http://localhost:58068/UserManagementService.svc")))
                     .SingleInstance();

            builder.Register(c => new ChannelFactory<ISubscriptionService>(
                     new BasicHttpBinding(),
                     new EndpointAddress("http://localhost:58068/SubscriptionManagementService.svc")))
                   .SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<IUserService>>().CreateChannel())
                   .As<IUserService>()
                   .UseWcfSafeRelease();

            builder.Register(c => c.Resolve<ChannelFactory<ISubscriptionService>>().CreateChannel())
                   .As<ISubscriptionService>()
                   .UseWcfSafeRelease();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            // build container
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

    }
}