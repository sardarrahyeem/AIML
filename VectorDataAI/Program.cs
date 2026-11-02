using System.ClientModel;
using CommunityToolkit.VectorData.InMemory;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;
using OpenAI;
using VectorDataAI;

List<CloudService> cloudServices =
[
    new() {
            Key = 0,
            Name = "Azure App Service",
            Description = "Host .NET, Java, Node.js, and Python web applications and APIs in a fully managed Azure service. You only need to deploy your code to Azure. Azure takes care of all the infrastructure management like high availability, load balancing, and autoscaling."
    },
    new() {
            Key = 1,
            Name = "Azure Service Bus",
            Description = "A fully managed enterprise message broker supporting both point to point and publish-subscribe integrations. It's ideal for building decoupled applications, queue-based load leveling, or facilitating communication between microservices."
    },
    new() {
            Key = 2,
            Name = "Azure Blob Storage",
            Description = "Azure Blob Storage allows your applications to store and retrieve files in the cloud. Azure Storage is highly scalable to store massive amounts of data and data is stored redundantly to ensure high availability."
    },
    new() {
            Key = 3,
            Name = "Microsoft Entra ID",
            Description = "Manage user identities and control access to your apps, data, and resources."
    },
    new() {
            Key = 4,
            Name = "Azure Key Vault",
            Description = "Store and access application secrets like connection strings and API keys in an encrypted vault with restricted access to make sure your secrets and your application aren't compromised."
    },
    new() {
            Key = 5,
            Name = "Azure AI Search",
            Description = "Information retrieval at scale for traditional and conversational search applications, with security and options for AI enrichment and vectorization."
    }
];


// Load the configuration values
var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string? model = config["ModelName"]; 
string? key = config["OpenAIKey"];

if (string.IsNullOrWhiteSpace(model))
    throw new InvalidOperationException("The 'ModelName' configuration value is missing.");

if (string.IsNullOrWhiteSpace(key))
    throw new InvalidOperationException("The 'OpenAIKey' configuration value is missing.");

// Create the embedding generator client
IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = 
     new OpenAIClient(new ApiKeyCredential(key))
    .GetEmbeddingClient(model: model).AsIEmbeddingGenerator(); 


// Create and Populate the vector store
var vectorStore = new InMemoryVectorStore();
VectorStoreCollection<int, CloudService> cloudServiceVectorStore = vectorStore.GetCollection<int, CloudService>("cloudServices");
await cloudServiceVectorStore.EnsureCollectionExistsAsync();

foreach (CloudService service in cloudServices)
{
    service.Vector = await embeddingGenerator.GenerateVectorAsync(service.Description);
    await cloudServiceVectorStore.UpsertAsync(service);
}


// Create a search query to a vector
// and search the vector store.
string query = "Which Azure service should I use to store my Word documents?";
ReadOnlyMemory<float> queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

IAsyncEnumerable<VectorSearchResult<CloudService>> results = cloudServiceVectorStore.SearchAsync(queryEmbedding, top: 1);


await foreach (VectorSearchResult<CloudService> result in results)
{
   
    Console.WriteLine($"Name: {result.Record.Name}");
    Console.WriteLine($"Description: {result.Record.Description}");
    Console.WriteLine($"Vector match score: {result.Score}");
}