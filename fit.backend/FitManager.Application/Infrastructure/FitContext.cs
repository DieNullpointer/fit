using Bogus;
using Bogus.DataSets;
using FitManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitManager.Application.Infrastructure
{
    public class FitContext : DbContext
    {
        public FitContext(DbContextOptions<FitContext> opt) : base(opt)
        {
        }

        public DbSet<Model.Company> Companies => Set<Model.Company>();
        public DbSet<Model.ContactPartner> ContactPartners => Set<Model.ContactPartner>();
        public DbSet<Model.Event> Events => Set<Model.Event>();
        public DbSet<Model.Package> Packages => Set<Model.Package>();

        public DbSet<Model.Config> Configs => Set<Model.Config>();

        public async Task<Model.Config> GetConfig() => (await Configs.OrderBy(c => c.Id).FirstOrDefaultAsync()) ?? new Model.Config();

        public async Task SetConfig(Config config)
        {
            if (config.Id == default) Configs.Add(config);
            await SaveChangesAsync();
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1039);
            var faker = new Faker("de");

            var packages = new List<Model.Package>
            {
                new Model.Package(name: "Messestand", price: 600M),
                new Model.Package(name: "Jahres-Inserat + Messestand", price: 1000M)
            };
            Packages.AddRange(packages);
            SaveChanges();

            var events = new Faker<Model.Event>("de").CustomInstantiator(f =>
            {
                var date = f.Date.Future(refDate: DateTime.UtcNow, yearsToGoForward: 5);
                var @event = new Model.Event(name: f.Name.JobTitle(), date: date.Date);
                @event.Packages.Add(f.PickRandom(packages));
                return @event;
            }).Generate(5).ToList();
            events.Add(new Model.Event(new DateTime(year: 2024, month: 3, day: 23), "FIT24"));
            events.Add(new Model.Event(new DateTime(year: 2023, month: 3, day: 23), "FIT23"));
            events.Add(new Model.Event(new DateTime(year: 2022, month: 3, day: 23), "FIT22"));
            Events.AddRange(events);
            SaveChanges();

            var companies = new Faker<Model.Company>("de").CustomInstantiator(f =>
            {
                return new Model.Company(name: f.Company.CompanyName(), address: f.Address.StreetAddress(), country: f.Address.Country(), plz: f.Address.ZipCode(), place: f.Address.City(), billAddress: f.Address.StreetAddress(), package: f.Random.ListItem(packages), @event: f.Random.ListItem(events))
                { Guid = faker.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            Companies.AddRange(companies);
            SaveChanges();

            var c = companies.ToList();

            var partners = new Faker<Model.ContactPartner>("de").CustomInstantiator(f =>
            {
                var company = f.PickRandom(c);
                c.Remove(company);
                return new Model.ContactPartner(title: f.Random.Word(), firstname: f.Person.FirstName, lastname: f.Person.LastName, email: f.Person.Email, telNr: f.Person.Phone, company: company, mainPartner: true, function: f.Name.JobTitle());
            }).Generate(10).ToList();
            ContactPartners.AddRange(partners);
            SaveChanges();

            var partners2 = new Faker<Model.ContactPartner>("de").CustomInstantiator(f =>
            {
                return new Model.ContactPartner(title: f.Random.Word(), firstname: f.Person.FirstName, lastname: f.Person.LastName, email: f.Person.Email, telNr: f.Person.Phone, company: f.PickRandom(companies), mainPartner: false, function: f.Name.JobTitle());
            }).Generate(10).ToList();
            ContactPartners.AddRange(partners2);
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Additional config

            // Generic config for all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }

        private void Initialize()
        {
            // Your code
        }

        /// <summary>
        /// Creates the database. Called once at application startup.
        /// </summary>
        public void CreateDatabase(bool isDevelopment)
        {
            if (isDevelopment) { Database.EnsureDeleted(); }
            // EnsureCreated only creates the model if the database does not exist or it has no
            // tables. Returns true if the schema was created.  Returns false if there are
            // existing tables in the database. This avoids initializing multiple times.
            if (Database.EnsureCreated()) { Initialize(); }
            if (isDevelopment) Seed();
        }

        /// <summary>
        /// Set the account to send alert emails and encrypt the refresh token with the key provided.
        /// </summary>
        public async Task SetMailerAccount(string mailAccountname, string refreshToken, byte[] key)
        {
            using var aes = Aes.Create();
            aes.Key = key;

            var value = Encoding.UTF8.GetBytes(refreshToken);
            using var memoryStream = new MemoryStream();
            memoryStream.Write(aes.IV);
            using var encryptor = aes.CreateEncryptor();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                cryptoStream.Write(value);
            var encryptedToken = Convert.ToBase64String(memoryStream.ToArray());

            var config = await GetConfig();
            config.MailerRefreshToken = encryptedToken;
            config.MailerAccountname = mailAccountname;
            await SetConfig(config);
        }

        /// <summary>
        /// Read the account to send alert emails and decrypt the refresh token with the key provided.
        /// </summary>
        public async Task<(string? accountname, string? refreshToken)> GetMailerAccount(byte[] key)
        {
            var config = await GetConfig();
            if (string.IsNullOrEmpty(config.MailerAccountname) || string.IsNullOrEmpty(config.MailerRefreshToken))
                return (null, null);

            using var aes = Aes.Create();
            var memoryStream = new MemoryStream(Convert.FromBase64String(config.MailerRefreshToken));
            var iv = new byte[aes.BlockSize / 8];
            memoryStream.Read(iv);

            using var decryptor = aes.CreateDecryptor(key, iv);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var dataStream = new MemoryStream();
            cryptoStream.CopyTo(dataStream);
            var decryptedToken = Encoding.UTF8.GetString(dataStream.ToArray());
            return (config.MailerAccountname, decryptedToken);
        }
    }
}