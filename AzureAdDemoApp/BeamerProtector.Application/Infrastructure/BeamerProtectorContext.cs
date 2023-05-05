using BeamerProtector.Application.Model;
using Bogus;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Core;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeamerProtector.Application.Infrastructure
{
    public class BeamerProtectorContext : DbContext
    {
        protected DbSet<Config> Configs => Set<Config>();
        public DbSet<Device> Devices => Set<Device>();

        public BeamerProtectorContext(DbContextOptions<BeamerProtectorContext> opt) : base(opt)
        {
        }

        public async Task<Config> GetConfig() => (await Configs.OrderBy(c => c.Id).FirstOrDefaultAsync()) ?? new Config();

        public async Task SetConfig(Config config)
        {
            if (config.Id == default) Configs.Add(config);
            await SaveChangesAsync();
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

        /// <summary>
        /// Initialize the database with some values (holidays, ...).
        /// Unlike Seed, this method is also called in production.
        /// </summary>
        private void Initialize()
        {
            // Seed logic.
        }

        /// <summary>
        /// Generates random values for testing the application. This method is only called in development mode.
        /// </summary>
        public void Seed()
        {
            Randomizer.Seed = new Random(1709);
            var faker = new Faker("de");

            var devices = new Faker<Device>("de").CustomInstantiator(f =>
            {
                return new Device(f.Random.Guid());
            })
            .Generate(5)
            .ToList();
            Devices.AddRange(devices);
            SaveChanges();
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
    }
}