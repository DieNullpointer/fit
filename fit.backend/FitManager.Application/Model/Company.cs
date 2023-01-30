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
        public Company(string name, string address, string country, string plz, string bIllAddress)
        {
            Name = name;
            Address = address;
            Country = country;
            Plz = plz;
            BillAddress = bIllAddress;
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
        public string BillAddress { get; set; }

        public List<ContactPartner> ContactPartners = new();
    }
}
