using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient client = new OllamaApiClient(
    new Uri("http://localhost:11434/"), "phi3:mini");

//Console.WriteLine(await client.GetResponseAsync("What is AI?"));

/*

Console.WriteLine(await client.GetResponseAsync([
    new ChatMessage(ChatRole.System, "You are a helpful assistant."),
    new ChatMessage(ChatRole.User, "What is AI?")
]));
*/

List<ChatMessage> history = [];
while (true)
{
    Console.Write("Q: ");    
    history.Add(new ChatMessage(ChatRole.User, Console.ReadLine()));

    //var response = await client.GetResponseAsync(history);
    //Console.WriteLine($"AI: {response}");

    Console.Write("AI: ");

    List<ChatResponseUpdate> updates = [];

    await foreach (ChatResponseUpdate update in client.GetStreamingResponseAsync(history))
    {
        Console.Write(update);
        updates.Add(update);           
        
    }

    Console.WriteLine();



    history.AddMessages(updates);
}