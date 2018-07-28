namespace Ocr.Infrastructure.Models.GoogleCloudVision
{
    public class Request
    {
        public Feature[] Features { get; set; }

        public Image Image { get; set; }
    }
}
