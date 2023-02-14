using FitManager.Application.Services;
using System.IO;
using Xunit;

namespace FitManager.Test;

public class PdfGeneratorTests
{
    public PdfGeneratorTests()
    {
        // Read Syncfusion key from appsettings.json
        var settings = Path.Combine("..", "..", "..", "..", "FitManager.Webapi", "appsettings.Development.json");
        var config = System.Text.Json.JsonDocument.Parse(File.ReadAllText(settings)).RootElement;

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(config.GetProperty("SyncfusionKey").GetString());
    }
    [Fact]
    public void GenerateInvoiceSuccessTest()
    {
        var service = new PdfGenerator();
        var result = service.GenerateInvoice();
        // Write file to fit\fit.backend\FitManager.Test\bin\Debug\net6.0
        File.WriteAllBytes("test.pdf", result);
        Assert.True(result.Length > 0);
    }
}