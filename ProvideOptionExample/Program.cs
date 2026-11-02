using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"));

client = ChatClientBuilderChatClientExtensions.AsBuilder(client)
     .ConfigureOptions(option => option.ModelId ??= "phi3")
     .Build();



// Will request "phi3".
//Console.WriteLine(await client.GetResponseAsync("What is AI?"));
// Will request "llama3.1".
Console.WriteLine($"llama3.1: {await client.GetResponseAsync("What is AI?", new() { ModelId = "llama3.1" })}");