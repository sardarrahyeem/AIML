using System.Threading.RateLimiting;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;

/*

var client = new RateLimitingChatClient(
    new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.1"),
    new ConcurrencyLimiter(new() { PermitLimit = 1, QueueLimit = int.MaxValue }));

    Console.WriteLine(await client.GetResponseAsync("What color is the sky?"));

*/
/*
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

IChatClient client = new OllamaApiClient(
    new Uri("http://localhost:11434/"),
    "phi3:mini");

builder.Services.AddChatClient(services =>
        client
        .AsBuilder()
        .UseDistributedCache()
        .UseRateLimiting(new ConcurrencyLimiter(new ConcurrencyLimiterOptions
        {
            PermitLimit = 1,
            QueueLimit = int.MaxValue
        }))
        .UseOpenTelemetry()
        .Build(services));

*/


//Console.WriteLine(await client.GetResponseAsync("What color is the sky?"));


// App setup.
var builder = Host.CreateApplicationBuilder();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddChatClient(new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.1"))
    .UseDistributedCache();
var host = builder.Build();

// Elsewhere in the app.
var chatClient = host.Services.GetRequiredService<IChatClient>();
Console.WriteLine(await chatClient.GetResponseAsync("What is AI?"));