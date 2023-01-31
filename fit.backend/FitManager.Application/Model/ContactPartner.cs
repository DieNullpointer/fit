using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Model
{
    public class ContactPartner
    {
        public ContactPartner(string title, string firstname, string lastname, string email, string telNr, string function, Company company, string? mobilNr = null, bool mainPartner = false)
        {
            Title = title;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            TelNr = telNr;
            MobilNr = mobilNr;
            Function = function;
            Company = company;
            MainPartner = mainPartner;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected ContactPartner() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string TelNr { get; set; }
        public string? MobilNr { get; set; }
        public string Function { get; set; }

        public bool MainPartner { get; set; }
        public Company Company { get; set; }
    }
}
