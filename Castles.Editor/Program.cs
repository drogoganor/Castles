using Autofac;
using Castles;
using Castles.Editor;

var builder = Castles.DependencyInjection.Build();
var container = builder.BuildEditorClient();
var client = container.Resolve<EditorClient>();
client.Run();
