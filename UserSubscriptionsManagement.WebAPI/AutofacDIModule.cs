﻿using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Contracts.ServiceContracts;

namespace UserSubscriptionsManagement.WebAPI
{
    public class AutofacDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
        }
    }
}
