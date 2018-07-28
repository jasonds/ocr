using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ocr.Core.Services;
using Ocr.Infrastructure.Services;

namespace Ocr.App
{
    public class Program
    {
        private static string _subscriptionKey;
        private static string _uriBase;

        public static void Main(string[] args)
        {
            // Configure application
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            IConfigurationRoot configuration = builder.Build();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // Configure dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton(configuration)
                //.AddScoped<IVisionService, AzureComputerVisionService>()
                .AddScoped<IVisionService, GoogleCloudVisionService>()
                .BuildServiceProvider();

            // Instantiate application
            IVisionService visionService = serviceProvider.GetService<IVisionService>();
            OcrApp app = new OcrApp(visionService);

            // Process OCR workload
            app.Process();
        }
    }
}
