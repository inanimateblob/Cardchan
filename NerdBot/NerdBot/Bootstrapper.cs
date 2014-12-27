﻿using System;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using NerdBot.Http;
using NerdBot.Messengers;
using NerdBot.Messengers.GroupMe;
using NerdBot.Mtg;
using Ninject;

namespace NerdBot
{
    using Nancy;

    public class Bootstrapper : NinjectNancyBootstrapper
    {
        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.
        }

        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            // Perform registation that should have an application lifetime

            existingContainer.Bind<IMtgContext>()
                .To<MtgContext>();

            existingContainer.Bind<IMtgStore>()
                .To<MtgStore>();

            existingContainer.Bind<IHttpClient>()
                .To<SimpleHttpClient>();

            existingContainer.Bind<IMessenger>()
                .To<GroupMeMessenger>()
                .WithConstructorArgument("botId", Properties.Settings.Default.BotId)
                .WithConstructorArgument("botName", Properties.Settings.Default.BotName)
                .WithConstructorArgument("ignoreNames", new string[] {})
                .WithConstructorArgument("endPointUrl", Properties.Settings.Default.EndPointUrl);
        }

        protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime
        }

        protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
        }
    }
}