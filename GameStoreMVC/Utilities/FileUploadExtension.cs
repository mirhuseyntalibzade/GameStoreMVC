namespace GameStoreMVC.Utilities
{
    public static class FileUploadExtension
    {
        public static async Task<string?> AddFileAsync(this IFormFile image, IWebHostEnvironment webHostEnvironment, string FolderName,long maxSize)
        {
            if (image == null || image.Length == 0)
            {
                throw new Exception("Requires image.");
            }
            if (image.Length > 5 * 1024 * 1024)
            {
                throw new Exception($"Image cannot exceed {maxSize}");
            }
            string rootPath = webHostEnvironment.WebRootPath;
            string fileName = $"{Guid.NewGuid()}_{image.FileName}";
            string folderPath = Path.Combine(rootPath, FolderName);
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string extension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception($"Only image files are supported");
            }
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Path.Combine(fileName);
        }
    }
}
