using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record PackageCmd
    (
            [StringLength(255, MinimumLength = 3, ErrorMessage = "Die Länge des Namens ist ungültig.")]
            string name,
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Die Länge des Preises ist ungültig")]
            string price
    );
}
