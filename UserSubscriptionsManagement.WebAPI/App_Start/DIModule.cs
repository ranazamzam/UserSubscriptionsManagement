using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using UserSubscriptionsManagement.Contracts.ServiceContracts;

namespace UserSubscriptionsManagement.WebAPI
{
    public class DIModule
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            // register services

            builder.Register(c => new ChannelFactory<IUserService>(
                       new BasicHttpBinding(),
                       new EndpointAddress("http://localhost:58068/UserManagementService")))
                     .SingleInstance();

            builder.Register(c => new ChannelFactory<ISubscriptionService>(
                     new BasicHttpBinding(),
                     new EndpointAddress("http://localhost:58068/SubscriptionManagementService")))
                   .SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<IUserService>>().CreateChannel())
                   .As<IUserService>()
                   .UseWcfSafeRelease();

            builder.Register(c => c.Resolve<ChannelFactory<ISubscriptionService>>().CreateChannel())
                   .As<ISubscriptionService>()
                   .UseWcfSafeRelease();


            // build container
            return builder.Build();
        }
    }
}