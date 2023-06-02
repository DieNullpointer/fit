using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Model
{
    public class Company
    {
        public Company(string name, string address, string country, string plz, string place, string billAddress, Package package, Event @event, string? description = null, bool hasPaid = false)
        {
            Name = name;
            Address = address;
            Country = country;
            Plz = plz;
            BillAddress = billAddress;
            Place = place;
            Package = package;
            Event = @event;
            Description = description;
            HasPaid = hasPaid;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Company() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Plz { get; set; }
        public string Place { get; set; }
        public string BillAddress { get; set; }
        public string? Description { get; set; }
        public bool HasPaid { get; set; }
        public string? LastPackage { get; set; } = null;
        public Package Package { get; set; }
        public Event Event { get; set; }

        public List<ContactPartner> ContactPartners { get; } = new();
    }
}
