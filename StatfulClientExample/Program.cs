using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OllamaSharp;
using OpenAI;

//IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.1");

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string? model = config["ModelName"];
string? key = config["OpenAIKey"];

// Create the IChatClient
IChatClient chatClient =
    new OpenAIClient(key).GetChatClient(model).AsIChatClient();

// Statelss Code
/*

List<ChatMessage> history = [];
while (true)
{
    Console.Write("Q: ");
    history.Add(new(ChatRole.User, Console.ReadLine()));

    var response = await client.GetResponseAsync(history);
    Console.WriteLine(response);

    history.AddMessages(response);
}
*/

// Statefull code

List<ChatMessage> chatHistory = [];

ChatOptions statefulOptoins = new() { ConversationId = "my-conversation-id" };

while (true)
{
    Console.Write("Q: ");
    ChatMessage message = new(ChatRole.User, Console.ReadLine());

    ChatResponse response = await chatClient.GetResponseAsync(message, statefulOptoins );

    Console.WriteLine(response);
    statefulOptoins.ConversationId = response.ConversationId;
    
    Console.WriteLine($"Conversation Id:{response.ConversationId}");

    if (response.ConversationId is not null)
    {
        chatHistory.Clear();
    }
    else
    {
        chatHistory.AddMessages(response);
    }
}