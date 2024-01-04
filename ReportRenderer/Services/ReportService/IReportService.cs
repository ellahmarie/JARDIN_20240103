namespace ReportRenderer.Services.ReportService
{
    public interface IReportService
    {
        public string GenerateFromFile(IFormFile dataset, IFormFile templateFile);
    }
}
