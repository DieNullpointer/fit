using BeamerProtector.Application.Infrastructure;
using BeamerProtector.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToLower();
}

internal class Program
{
    private static readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions { PropertyNamingPolicy = new LowerCaseNamingPolicy() };
    private static readonly string _database = "../BeamerProtector.Webapp/BeamerProtector.db";

    /// <summary>
    /// Sends an object to /api/measures as a POST request with JSON body.
    /// </summary>
    private static async Task SendData<T>(HttpClient client, T data, CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _serializationOptions);
            Console.WriteLine($"Send {json}...");
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("measures", payload, cancellationToken);
            response.EnsureSuccessStatusCode();
            Console.WriteLine($"OK");
        }
        catch (HttpRequestException e)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine($"Error sending HTTP Request: {e.Message}. Check if the API is up and running.");
            Console.ForegroundColor = color;
        }
    }

    /// <summary>
    /// Read the devices from the database. We use the database from the webapp, so we can send
    /// existing device guids.
    /// </summary>
    private static async Task<IReadOnlyList<Device>> GetDevicesFromDatabase()
    {
        try
        {
            if (!File.Exists(_database))
                throw new ApplicationException($"Database {Path.GetFullPath(_database)} not found. Start the Webapp to create the Database.");
            using var context = new BeamerProtectorContext(new DbContextOptionsBuilder<BeamerProtectorContext>()
                .UseSqlite($"DataSource={_database}")
                .Options);
            var devices = await context.Devices.ToListAsync();
            if (!devices.Any())
                throw new ApplicationException("No devices found in the Database.");
            return devices;
        }
        catch (Exception e)
        {
            throw new ApplicationException(e.InnerException?.Message ?? e.Message, e);
        }
    }

    private static async Task<int> Main(string[] args)
    {
        var cancellationToken = new CancellationTokenSource();

        // Cancel if CTRL+C is pressed.
        Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
        {
            cancellationToken.Cancel();
            e.Cancel = true;
        };

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:5001/api/");

        try
        {
            var devices = await GetDevicesFromDatabase();
            Console.WriteLine($"Found {devices.Count} devices in database.");
            while (!cancellationToken.IsCancellationRequested)
            {
                await SendData(client, new { Device = devices[0].Guid, Pinstate = 1 }, cancellationToken.Token);
                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken.Token);
            }
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Cancelled by user.");
        }
        catch (Exception e)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine(e.Message);
            Console.ForegroundColor = color;
            return 1;
        }
        return 0;
    }
}