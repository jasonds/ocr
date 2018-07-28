using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

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

            // Store settings
            _subscriptionKey = (configuration[Constants.AppSettings.ComputerVisionSubscriptionKey]);
            _uriBase = (configuration[Constants.AppSettings.ComputerVisionOcrUriBase]);

            // Get the path and filename to process from the user
            Console.WriteLine("Optical Character Recognition:");
            Console.Write("Enter the path to an image with text you wish to read: ");
            string imageFilePath = Console.ReadLine();

            if (File.Exists(imageFilePath))
            {
                // Make the REST API call
                Console.WriteLine("Wait a moment for the results to appear.");
                MakeOcrRequest(imageFilePath).Wait();
            }
            else
            {
                Console.WriteLine("Invalid file path");
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Gets the text visible in the specified image file by using the Computer Vision REST API
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text</param>
        private static async Task MakeOcrRequest(string imageFilePath)
        {
            string result;

            try
            {
                HttpClient client = new HttpClient();

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                // Request parameters
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call
                string uri = _uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                result = JToken.Parse(contentString).ToString();

                await File.WriteAllTextAsync(@"C:\Users\jsears\Desktop\test2_azure.json", result);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            Console.WriteLine("Response:\n\n{0}\n", result);
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array
        /// </summary>
        /// <param name="imageFilePath">The image file to read</param>
        /// <returns>The byte array of the image data</returns>
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
