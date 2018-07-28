using System;
using System.IO;
using System.Threading.Tasks;
using Ocr.Core.Services;

namespace Ocr.App
{
    public class OcrApp
    {
        private readonly IVisionService _visionService;

        public OcrApp(IVisionService visionService)
        {
            if (visionService == null) throw new ArgumentNullException(nameof(visionService));
            _visionService = visionService;
        }

        public void Process()
        {
            // Get the path and filename to process from the user
            Console.WriteLine("Optical Character Recognition:");
            Console.Write("Enter the path to an image with text you wish to read: ");
            string imageFilePath = Console.ReadLine();

            if (File.Exists(imageFilePath))
            {
                // Make the REST API call
                Console.WriteLine("Wait a moment for the results to appear.");

                Task<string> task = Task.Run(async () => await _visionService.GetAnnotations(imageFilePath));
                string result = task.Result;

                Console.WriteLine("Response:\n\n{0}\n", result);
            }
            else
            {
                Console.WriteLine("Invalid file path");
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
