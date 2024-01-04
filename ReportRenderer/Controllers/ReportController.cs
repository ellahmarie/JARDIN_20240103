using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportRenderer.Extensions;
using ReportRenderer.Services;
using ReportRenderer.Services.ReportService;
using System.Data;
using System.Text;

namespace ReportRenderer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;
        public ReportController(IReportService reportService) { 
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var form = await this.HttpContext.Request.ReadFormAsync();
                var dataset = form.Files.FirstOrDefault(e => e.Name == "dataset");

                if (dataset == null)
                {
                    return BadRequest("Dataset not found.");
                }

                var template = form.Files.FirstOrDefault(e => e.Name == "template");
                if (template == null)
                {
                    return BadRequest("Template not found.");
                }

                return Ok(reportService.GenerateFromFile(dataset, template));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
