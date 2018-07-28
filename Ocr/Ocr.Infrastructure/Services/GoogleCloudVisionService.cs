using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocr.Core.Services;
using Ocr.Infrastructure.Models.GoogleCloudVision;

namespace Ocr.Infrastructure.Services
{
    public class GoogleCloudVisionService : BaseVisionService, IVisionService
    {
        public GoogleCloudVisionService(IConfiguration configurationService)
            : base(configurationService)
        {
        }

        public async Task<string> GetAnnotations(string imageFilePath)
        {
            string result;

            try
            {
                HttpClient client = new HttpClient();
                
                // Request parameters
                string requestParameters = $"key={_apiKey}";

                // Assemble the URI for the REST API Call
                string uri = _uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image
                Payload payload = new Payload
                {
                    Requests = new[]
                    {
                        new Request
                        {
                            Image = new Image
                            {
                                Content = Convert.ToBase64String(GetImageAsByteArray(imageFilePath))
                            },
                            Features = new[]
                            {
                                new Feature
                                {
                                    Type = "TEXT_DETECTION"
                                }
                            }
                        }
                    }
                };

                string payloadContent = JsonConvert.SerializeObject(payload);
                using (StringContent content = new StringContent(payloadContent))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
