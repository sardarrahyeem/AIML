// Configure OpenTelemetry exporter
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OllamaSharp;
using OpenTelemetry.Trace;

string sourceName = Guid.NewGuid().ToString();
TracerProvider tracerProvider = OpenTelemetry.Sdk.CreateTracerProviderBuilder()
    .AddSource(sourceName)
    .AddConsoleExporter()
    .Build();

IChatClient ollamaClient = new OllamaApiClient(new Uri("http://localhost:11434/"), "phi3:mini");

IChatClient client = new ChatClientBuilder(ollamaClient)
    .UseDistributedCache(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())))
    .UseFunctionInvocation()
    .UseOpenTelemetry(
        sourceName: sourceName,
        configure: c => c.EnableSensitiveData = true)
    .Build();

Console.WriteLine((await client.GetResponseAsync("What is AI?")).Text);


