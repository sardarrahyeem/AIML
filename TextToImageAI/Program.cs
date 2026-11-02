using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string? model = config["ModelName"];
string? key = config["OpenAIKey"];

// Use AsIImageGenerator() to bind the client to the unified ecosystem

#pragma warning disable MEAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
IImageGenerator generator = new OpenAIClient(key)
    .GetImageClient(model ?? "dall-e-3")
    .AsIImageGenerator();


 string prompt = "A cute kitten playing soccer on the moon, comic book style";

        Console.WriteLine("Generating image via MEAI abstraction...");
      

       try
        {
            // Generate images and parse the underlying data primitive
            var response = await generator.GenerateImagesAsync(prompt);
            
            // Extract the generated raw data content safely
            var image = response.Contents.OfType<DataContent>().FirstOrDefault();
            if (image is not null && image.Data.Length > 0)
            {
                byte[] imageBytes = image.Data.ToArray();
                await File.WriteAllBytesAsync("moon_cat.png", imageBytes);
                Console.WriteLine("Image saved successfully to moon_cat.png!");
            }
            else
            {
                Console.WriteLine("No image data returned from the provider.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

       
#pragma warning restore MEAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

