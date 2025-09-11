namespace BookStore.Api.Helpers
{
    public class FileManager
    {
        private readonly IWebHostEnvironment webHost;

        public FileManager(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }
        public async Task<string?> UploadFile(IFormFile? file, string folderName)
        {
            if (file == null)
                return null;
            var folderPath = Path.Combine(webHost.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);
            using var fStreem = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fStreem);
            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(webHost.WebRootPath, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
