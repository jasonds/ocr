using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Ocr.Core.Services;

namespace Ocr.Infrastructure.Services
{
    public class AzureComputerVisionService : BaseVisionService, IVisionService
    {
        public AzureComputerVisionService(IConfigurationRoot configurationService)
            : base(configurationService)
        {
        }

        public async Task<string> GetAnnotations(string imageFilePath)
        {
            string result;

            try
            {
                HttpClient client = new HttpClient();

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);

                // Request parameters
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call
                string uri = _uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response
                string contentString = await response.Content.ReadAsStringAsync();
                result = JToken.Parse(contentString).ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
