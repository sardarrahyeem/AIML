using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string? model = config["ModelName"];
string? key = config["OpenAIKey"];

IChatClient client = new ChatClientBuilder(new OpenAIClient(key).GetChatClient(model ?? "gpt-5").AsIChatClient())
.UseFunctionInvocation()
.Build();

Delegate getWeatherDelegate = (AIFunctionArguments args) =>
{
// Access named parameters from the arguments dictionary.
string? location = args.TryGetValue("location", out object? loc) ? 
loc.ToString() : "Unknown";
string? units = args.TryGetValue("units", out object? u) ? u.ToString() : 
"celsius";
return $"Weather in {location}: 35°{units}";
};

// Create the AIFunction.
AIFunction getWeather = AIFunctionFactory.Create(getWeatherDelegate);
// Call the function manually.
var result = await getWeather.InvokeAsync(new AIFunctionArguments
{
{ "location", "Seattle" },
{ "units", "F" }
});
Console.WriteLine($"Function result: {result}");

