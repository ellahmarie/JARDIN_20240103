using HtmlAgilityPack;
using Newtonsoft.Json;
using ReportRenderer.Extensions;
using System.Data;
using System.Text;

namespace ReportRenderer.Services
{
    public class ReportService
    {
        private readonly string TEMPLATE_ROW_TAG = "template-row";
        private readonly string PREFIX_TAG = "field-";

        public string GenerateFromFile(IFormFile dataset, IFormFile templateFile)
        {
            string jsonDataset = dataset.toJsonString();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(templateFile.toJsonString());

            var templateRow = htmlDoc.DocumentNode.SelectSingleNode(this.TEMPLATE_ROW_TAG).InnerHtml;
            var fieldTags = htmlDoc.DocumentNode.SelectSingleNode(this.TEMPLATE_ROW_TAG).Descendants()
                    .Where(n => n.NodeType == HtmlNodeType.Element)
                    .Select(n => n.Name.ToLower())
                    .Distinct()
                    .ToList();


            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonDataset);

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
                stringBuilder.Append(reportRow);
            }

            return stringBuilder.ToString();
        }

    }
}
