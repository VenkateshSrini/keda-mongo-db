using mongodb.job;
using mongodb.job.ServiceExtension;
using System.Diagnostics;
var isDebugger = Debugger.IsAttached;
var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddMongoRepo(builder.Configuration, "MongoDB", "connectionString");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
if (isDebugger)
{
    host.Run();
}
else
{
    host.Start();
    host.StopAsync().Wait();
}

