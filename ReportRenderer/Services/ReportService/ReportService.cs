using HtmlAgilityPack;
using Newtonsoft.Json;
using ReportRenderer.Extensions;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace ReportRenderer.Services.ReportService
{
    public class ReportService: IReportService
    {
        private readonly string TEMPLATE_ROW_TAG = "template-row";
        private readonly string PREFIX_TAG = "field-";

        public string GenerateFromFile(IFormFile dataset, IFormFile templateFile)
        {
            string datasetContent = dataset.content();

            if (string.IsNullOrEmpty(datasetContent))
            {
                throw new Exception("Dataset file is empty.");
            }

            string templateContent = templateFile.content();
            if (string.IsNullOrEmpty(templateContent))
            {
                throw new Exception("Template file is empty.");
            }

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(templateContent);

            var templateRow = htmlDoc.DocumentNode.SelectSingleNode(this.TEMPLATE_ROW_TAG)?.InnerHtml;

            if (string.IsNullOrEmpty(templateRow))
            {
                throw new Exception("Template row not found.");
            }

            var fieldTags = htmlDoc.DocumentNode.SelectSingleNode(this.TEMPLATE_ROW_TAG).Descendants()
                    .Where(n => n.NodeType == HtmlNodeType.Element)
                    .Select(n => n.Name.ToLower())
                    .Distinct()
                    .ToList();

            if (!fieldTags.Any())
            {
                throw new Exception("No field tags found in the template.");
            }

            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(datasetContent);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (DataRow row in dataTable.Rows)
            {
                var reportRow = templateRow;

                fieldTags.ForEach(tag =>
                {
                    var cleanTag = tag.Replace(this.PREFIX_TAG, "");
                    if (row.Table.Columns.Contains(cleanTag))
                    {
                        reportRow = reportRow.Replace($"</{tag}>", "").Replace($"<{tag}>", row[cleanTag].ToString());
                    }
                });

                stringBuilder.Append(reportRow.Replace("\\n", ""));
            }

            return stringBuilder.ToString();
        }

    }
}
