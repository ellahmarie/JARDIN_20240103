using System.Text;

namespace ReportRenderer.Extensions
{
    public static class FileExtension
    {
        public static string content(this IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                string result = Encoding.UTF8.GetString(bytes);

                return result;
            }
        }

        public static bool isValidDataset(this IFormFile file)
        {
            var aa = Path.GetExtension(file.FileName);
            return Path.GetExtension(file.FileName) == ".json";
        }

        public static bool isValidTemplate(this IFormFile file)
        {
            return Path.GetExtension(file.FileName) == ".txt";
        }
    }
}
