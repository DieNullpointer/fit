using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Model
{
    public class Company
    {
        public Company(string name)
        {
            Name = name;
        }

        public int Id { get; private set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
