﻿using Autofac;
using Castles.Providers;
using Castles.SampleBase;
using Castles.UI;

namespace Castles
{
    public static class DependencyInjection
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<Startup>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<SettingsProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<VeldridStartupWindow>()
                .As<IApplicationWindow>()
                .SingleInstance();

            builder
                .RegisterType<Camera>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<MainMenu>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<Game>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            //builder.RegisterInstance(new VeldridStartupWindow("Castles"))
            //       .As<IApplicationWindow>();

            //builder.RegisterType<TaskController>();

            // Scan an assembly for components
            //builder.RegisterAssemblyTypes(myAssembly)
            //       .Where(t => t.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces();

            return builder.Build();
        }
    }
}