using System.Threading.Tasks;

namespace Ocr.Core.Services
{
    public interface IVisionService
    {
        Task<string> GetAnnotations(string imageFilePath);
    }
}
