using Microsoft.Extensions.AI;
using OllamaSharp;

IEmbeddingGenerator<string, Embedding<float>> generator = 
     new OllamaApiClient(new Uri("http://localhost:11434"),"nomic-embed-text");


foreach(Embedding<float> embedding in await generator.GenerateAsync(["What is AI?", "What is .NET?"]))
{
    Console.WriteLine(string.Join(",", embedding.Vector.ToArray()));
}