﻿using Bogus;
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
        public void Seed()
        {
            Randomizer.Seed = new Random(1039);
            var faker = new Faker("de");

            var companies = new Faker<Model.Company>("de").CustomInstantiator(f =>
            {
                return new Model.Company(name: f.Company.CompanyName())
                { Guid = faker.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            Companies.AddRange(companies);
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
