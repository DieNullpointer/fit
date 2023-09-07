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
using Syncfusion.Drawing;
using Novell.Directory.Ldap.Utilclass;
using System.Runtime.ConstrainedExecution;

namespace FitManager.Application.Services
{
    public class PdfGenerator
    {
        private static string _fontfile = Path.Combine("..", "FitManager.Application", "PdfTemplates", "Ubuntu-Regular.ttf");
        private static string _invoiceTemplate = Path.Combine("..", "FitManager.Application", "PdfTemplates", "briefpapier.pdf");

        private static string _invite = Path.Combine("..", "FitManager.Application", "PdfTemplates", "invite.pdf");
        private static string _calibriRegular = Path.Combine("..", "FitManager.Application", "PdfTemplates", "calibri-regular.ttf");
        private static string _calibriBold = Path.Combine("..", "FitManager.Application", "PdfTemplates", "calibri-bold.ttf");
        private static string _calibriBoldItalic = Path.Combine("..", "FitManager.Application", "PdfTemplates", "calibri-bold-italic.ttf");
        private static string _calibriItalic = Path.Combine("..", "FitManager.Application", "PdfTemplates", "calibri-italic.ttf");
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

        public byte[] GenerateInvite(string eventName, DateTime date)
        {
            if (!File.Exists(_invite))
                throw new PdfGeneratorException($"Templatefile {_invite} does not exist.");
            if (!File.Exists(_calibriBold))
                throw new PdfGeneratorException($"Fontfile {_calibriBold} does not exist.");
            if (!File.Exists(_calibriRegular))
                throw new PdfGeneratorException($"Fontfile {_calibriRegular} does not exist.");
            if (!File.Exists(_calibriBoldItalic))
                throw new PdfGeneratorException($"Fontfile {_calibriBoldItalic} does not exist.");
            if (!File.Exists(_calibriItalic))
                throw new PdfGeneratorException($"Fontfile {_calibriItalic} does not exist.");


            var fontBold11 = new PdfTrueTypeFont(_calibriBold, 11);
            var fontBold12 = new PdfTrueTypeFont(_calibriBold, 12);
            var font = new PdfTrueTypeFont(_calibriRegular, 11);

            using var infile = new FileStream(_invite, FileMode.Open, FileAccess.Read);
            using PdfLoadedDocument doc = new PdfLoadedDocument(infile);
            using PdfDocument pdfDocument = new PdfDocument();
            var page = doc.Pages[0];//pdfDocument.ImportPage(doc, 0);
            page.Graphics.DrawString(eventName, fontBold11, PdfBrushes.Black, 285, 250);
            string? weekDay = null;
            if (date.DayOfWeek.ToString() == "Monday")
                weekDay = "Montag";
            if (date.DayOfWeek.ToString() == "Tuesday")
                weekDay = "Dienstag";
            if (date.DayOfWeek.ToString() == "Wednesday")
                weekDay = "Mittwoch";
            if (date.DayOfWeek.ToString() == "Thursday")
                weekDay = "Donnerstag";
            if (date.DayOfWeek.ToString() == "Friday")
                weekDay = "Freitag";
            if (date.DayOfWeek.ToString() == "Saturday")
                weekDay = "Samstag";
            if (date.DayOfWeek.ToString() == "Sunday")
                weekDay = "Sonntag";
            page.Graphics.DrawString($"{weekDay}, den {date.Day}. {date.ToString("MMMM")} {date.Year}", fontBold11, PdfBrushes.Red, 235, 263);
            page.Graphics.DrawString($"{date.ToString("MMMM")} {date.Year}", font, PdfBrushes.Black, 460, 118);
            page.Graphics.DrawString($"{date.Year}", fontBold11, PdfBrushes.Black, 358, 591 );
            using var stream = new MemoryStream();
            doc.Save(stream);

            var page2 = doc.Pages[1];
            page2.Graphics.DrawString($"zum Firmeninformationstag {eventName}, am {weekDay}, den {date.ToString("d")} an der HTL für Textilindustrie und", fontBold12, PdfBrushes.Black, 50, 105);
            page2.Graphics.DrawString($"Datenverarbeitung, Spengergasse 20, 1050 Wien in der Zeit von 9‐18 Uhr.", fontBold12, PdfBrushes.Black, 100, 120);
            page2.Graphics.DrawString($"{date.Year}", font, PdfBrushes.Black, 233, 536.5f);
            page2.Graphics.DrawString($"{date.Year} Wird auf der Website publiziert)", font, PdfBrushes.Black, 171, 551.5f);
            //Wird auf der Website publiziert)
            //pdfDocument.Save(stream);
            doc.Save(stream);
            var content = stream.ToArray();
            return content;
        }
    }
}
