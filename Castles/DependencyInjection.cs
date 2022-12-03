using Autofac;
using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.SampleBase;
using Castles.Screens;
using Castles.Shaders;
using Castles.UI;

namespace Castles
{
    public static class DependencyInjection
    {
        public static ContainerBuilder Build()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<FileSystem>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<SettingsProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<ModManifestProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<Sdl2WindowProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<GraphicsDeviceProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<GameResourcesProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<GameMapProvider>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<VeldridStartupWindow>()
                .As<IApplicationWindow>()
                .SingleInstance();

            builder
                .RegisterType<Camera2D>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<TextureShader>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<ColorShader>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DebugGrid>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<Scene>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<ImGuiProvider>()
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

            return builder;
        }
    }
}
