using Autofac;
using Castles.UI;

namespace Castles.Editor
{
    public static class DependencyInjection
    {
        public static IContainer BuildEditorClient(this ContainerBuilder builder)
        {
            builder
                .RegisterType<EditorMainMenu>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<EditorNewMapMenu>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<EditorUI>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<EditorClient>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder.Build();
        }
    }
}
