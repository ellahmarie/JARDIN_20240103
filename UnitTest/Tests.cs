using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using ReportRenderer.Services.ReportService;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace UnitTest
{
    public class Tests
    {
        IReportService reportService;
        IFormFile dataset;
        IFormFile template;
        IFormFile dataset_empty;
        IFormFile template_no_templaterow_tag;
        IFormFile template_empty;
        IFormFile template_empty_tags;
        [SetUp]
        public void Setup()
        {
            dataset = ConvertFileToIFormFile("TestFiles\\Dataset.txt", "dataset");
            template = ConvertFileToIFormFile("TestFiles\\Template.txt", "template");
            dataset_empty = ConvertFileToIFormFile("TestFiles\\Empty-dataset.txt", "dataset");
            template_no_templaterow_tag = ConvertFileToIFormFile("TestFiles\\Template-no-templaterow-tag.txt", "template");
            template_empty = ConvertFileToIFormFile("TestFiles\\Empty-template.txt", "template");
            template_empty_tags = ConvertFileToIFormFile("TestFiles\\Template-empty-tags.txt", "template");
            reportService = new ReportService();
        }

        [Test]
        public void VerifyRow()
        {
            var report = this.reportService.GenerateFromFile(this.dataset, this.template);
            Assert.That(report.Contains("My name is Ellah Marie and i am 25 years old."));
        }

        [Test]
        public void VerifyEmptyDataset()
        {
            try
            {
                var report = this.reportService.GenerateFromFile(this.dataset_empty, this.template);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Dataset file is empty.");
            }
        }

        [Test]
        public void VerifyTemplateWithNoTemplateRowTag()
        {
            try
            {
                var report = this.reportService.GenerateFromFile(this.dataset, this.template_no_templaterow_tag);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Template row not found.");
            }
        }

        [Test]
        public void VerifyEmptyTemplate()
        {
            try
            {
                var report = this.reportService.GenerateFromFile(this.dataset, this.template_empty);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Template file is empty.");
            }
        }

        [Test]
        public void VerifyEmptyTags()
        {
            try
            {
                var report = this.reportService.GenerateFromFile(this.dataset, this.template_empty_tags);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "No field tags found in the template.");
            }
        }

        private IFormFile ConvertFileToIFormFile(string path, string name)
        {
            var stream = File.OpenRead(path);
            return new FormFile(stream, 0, stream.Length, name, name);
        }
    }
}