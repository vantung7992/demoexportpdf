using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DemoExportPdf.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public async Task GeneratePdf(string html, string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    PdfSharp.Pdf.PdfDocument pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                    pdf.Save(ms);
                    ms.Close();
                }
                Spire.Pdf.PdfDocument abc = new Spire.Pdf.PdfDocument(filePath);
                Spire.Pdf.Graphics.PdfImage image = Spire.Pdf.Graphics.PdfImage.FromFile(@"D:\Logo.png");
                float width = image.Width * 0.75f;
                float height = image.Height * 0.75f;

                Spire.Pdf.PdfPageBase page0 = abc.Pages[0];
                float x = (page0.Canvas.ClientSize.Width - width) / 2;
                page0.Canvas.DrawImage(image, x, 60, width, height);
                abc.SaveToFile(filePath);
            });
        }

        public async Task GeneratePdf2(string html, string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
               
                    XImage xImage = XImage.FromFile(@"D:\Logo.png");
                    PdfSharp.Pdf.PdfDocument pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                    PdfPage page = pdf.Pages[0];
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(xImage,0,0,xImage.PixelWidth,xImage.PixelHeight);
                 
                    pdf.Save(ms);
                }
            });
        }

        public void AddLogo(XGraphics gfx, string imagePath, int xPosition, int yPosition)
        {
            if (!System.IO.File.Exists(imagePath))
            {
                throw new FileNotFoundException(String.Format("Could not find image {0}.", imagePath));
            }

            XImage xImage = XImage.FromFile(imagePath);
            gfx.DrawImage(xImage, xPosition, yPosition, xImage.PixelWidth, xImage.PixelWidth);
        }

        public async Task<FileResult> Download()
        {
            var template = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("../Views/DemoExportPdf.html"));
            string fileName = "demoExportPdf.pdf";
            var filePath = System.Web.HttpContext.Current.Server.MapPath("../kq");
            string fullPath = Path.Combine(filePath, fileName);
            await GeneratePdf2(template, fullPath);
            return File(fullPath, "application/pdf", "demoExportPdf.pdf");
        }
    }
}