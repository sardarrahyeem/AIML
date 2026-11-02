// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.AI;

Console.WriteLine("Hello, World!");
Console.WriteLine("Hello, Microsoft Extensions AI Services");
var prompt = "Write an email to the .NET team, thanking them for creating the wonderful AI Library.";
IChatClient client = new ChatClient();
