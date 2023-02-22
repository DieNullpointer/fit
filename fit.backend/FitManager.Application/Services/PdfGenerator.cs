using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Ocsp;
using Syncfusion.Pdf.Barcode;

namespace FitManager.Application.Services
{
    public class PdfGenerator
    {
        private static string _fontfile = Path.Combine("..", "FitManager.Application", "PdfTemplates", "Ubuntu-Regular.ttf");
        private static string _invoiceTemplate = Path.Combine("..", "FitManager.Application", "PdfTemplates", "briefpapier.pdf");

        /// <summary>
        /// Generates a PDF Document
        /// This method returns a byte array. You can return a file download as a FileResult
        /// in your ASP.NET Core controller with
        ///     return File(content, "application/pdf", "your_filename.pdf")
        /// 
        /// Do not forget to set the licence code in appsettings.Development.json (as "SyncfusionKey")
        /// See https://help.syncfusion.com/file-formats/pdf/overview for detailled info about
        /// the Syncfusion PDF generator.
        /// 
        /// TODO: Add a DTO class (or a list of data) as an argument to draw the information.
        /// </summary>
        public byte[] GenerateInvoice()
        {
            if (!File.Exists(_invoiceTemplate))
                throw new PdfGeneratorException($"Templatefile {_invoiceTemplate} does not exist.");
            if (!File.Exists(_fontfile))
                throw new PdfGeneratorException($"Fontfile {_fontfile} does not exist.");

            using var infile = new FileStream(_invoiceTemplate, FileMode.Open, FileAccess.Read);
            using PdfLoadedDocument doc = new PdfLoadedDocument(infile);
            using PdfDocument pdfDocument = new PdfDocument();
            var font = new PdfTrueTypeFont(_fontfile, 12);

            var page = pdfDocument.ImportPage(doc, 0);
            page.Graphics.DrawString("A test", font, PdfBrushes.Black, 160, 313);

            // Be careful, Azure servers runs in UTC. so local time will return the UTC time.
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));
            page.Graphics.DrawString(localTime.ToString("dd.MM.yyyy HH:mm"), font, PdfBrushes.Black, 160, 411);

            using var stream = new MemoryStream();
            pdfDocument.Save(stream);
            var content = stream.ToArray();
            return content;
        }
    }
}
