using Autofac;

namespace Castles.Game
{
    public static class DependencyInjection
    {
        public static IContainer BuildGameClient(this ContainerBuilder builder)
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
