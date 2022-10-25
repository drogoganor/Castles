using Autofac;
using Castles;

var container = DependencyInjection.Build();
var startup = container.Resolve<Startup>();
startup.Run();
