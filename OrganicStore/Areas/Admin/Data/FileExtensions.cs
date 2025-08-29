namespace OrganicStore.Areas.Admin.Data
{
    public static class FileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool IsAllowedSize(this IFormFile file, int mb)
        {
            return file.Length <= mb * 1024 * 1024;
        }

        public static async Task<string> GenerateFileAsync(this IFormFile file, string basePath)
        {
            var unicalName = $"{Path.GetFileNameWithoutExtension(file.FileName)}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var unicalPath = Path.Combine(basePath, unicalName);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var fs = new FileStream(unicalPath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();

            return unicalName;
        }
    }
}