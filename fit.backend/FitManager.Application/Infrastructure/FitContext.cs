using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Infrastructure
{
    public class FitContext : DbContext
    {
        public FitContext(DbContextOptions<FitContext> opt): base(opt) { }
        public DbSet<Model.Company> Companies => Set<Model.Company>();
        public DbSet<Model.ContactPartner> ContactPartners => Set<Model.ContactPartner>();
        public DbSet<Model.Event> Events => Set<Model.Event>();
        public void Seed()
        {
            Randomizer.Seed = new Random(1039);
            var faker = new Faker("de");

            var companies = new Faker<Model.Company>("de").CustomInstantiator(f =>
            {
                return new Model.Company(name: f.Company.CompanyName(), address: f.Address.StreetAddress(), country: f.Address.Country(), plz: f.Address.ZipCode(), bIllAddress: f.Address.StreetAddress())
                { Guid = faker.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            Companies.AddRange(companies);
            SaveChanges();

            var partners = new Faker<Model.ContactPartner>("de").CustomInstantiator(f =>
            { 
                return new Model.ContactPartner(title: f.Random.Word(), firstname: f.Person.FirstName, lastname: f.Person.LastName, email: f.Person.Email, telNr: f.Person.Phone, company: f.PickRandom(companies), mainPartner: true, function: f.Name.JobTitle());
            }).Generate(10).ToList();
            ContactPartners.AddRange(partners);
            SaveChanges();

            var events = new Faker<Model.Event>("de").CustomInstantiator(f =>
            {
                return new Model.Event(name: f.Name.JobTitle(), date: f.Date.Future(refDate: DateTime.UtcNow, yearsToGoForward: 5));
            }).Generate(5).ToList();
            Events.AddRange(events);
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
    }
}
