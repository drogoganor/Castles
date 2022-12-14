using Autofac;
using Castles;
using Castles.Game;

var builder = Castles.DependencyInjection.Build();
var container = builder.BuildGameClient();
var client = container.Resolve<GameClient>();
client.Run();
