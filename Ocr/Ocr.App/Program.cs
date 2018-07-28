using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Ocr.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            IConfigurationRoot configuration = builder.Build();

            Console.WriteLine(configuration["subscriptionKey"]);
            Console.ReadLine();
        }
    }
}
