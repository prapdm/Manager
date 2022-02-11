using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Manager.Services
{
    public interface IFileService
    {
        Task UploadAsync(IFormFile file, string path);
    }


    public class FileService : IFileService
    {
        public FileService()
        {

        }

        public async Task UploadAsync(IFormFile file, string path)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Path.GetFullPath("wwwroot");
                var fileName = file.FileName;
                var fullPath = @$"{rootPath}\{path}\{fileName}";
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }
    }

   
}
