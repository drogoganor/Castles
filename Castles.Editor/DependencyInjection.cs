using Autofac;

namespace Castles.Editor
{
    public static class DependencyInjection
    {
        public static IContainer BuildEditorClient(this ContainerBuilder builder)
        {
            builder
                .RegisterType<GameClient>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder.Build();
        }
    }
}
