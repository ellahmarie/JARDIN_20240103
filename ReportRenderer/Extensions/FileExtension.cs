using System.Text;

namespace ReportRenderer.Extensions
{
    public static class FileExtension
    {
        public static string toJsonString(this IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                string result = Encoding.UTF8.GetString(bytes);

                return result;
            }
        }
    }
}
