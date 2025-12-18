// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stargazer.Orleans.Template.Silo;

var builder = Host.CreateDefaultBuilder(args).ConfigureLogging((context, logging) => logging.ClearProviders());

using IHost host = builder.ConfigureOrleansServer().Build();

await host.StartAsync();

Console.ReadKey();
await host.StopAsync();