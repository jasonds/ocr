using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Ocr.Core;

namespace Ocr.Infrastructure.Services
{
    public class BaseVisionService
    {
        private readonly IConfiguration _configurationService;

        protected readonly string _apiKey;
        protected readonly string _uriBase;

        public BaseVisionService(IConfiguration configurationService)
        {
            if (configurationService == null) throw new ArgumentNullException(nameof(configurationService));
            _configurationService = configurationService;

            _apiKey = (_configurationService[Constants.AppSettings.ApiKey]);
            _uriBase = (_configurationService[Constants.AppSettings.UriBase]);
        }

        protected byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
