using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record PackageDto
    (
            Guid Guid,
            [StringLength(255, MinimumLength = 3, ErrorMessage = "Die Länge des Namens ist ungültig.")]
            string Name,
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Die Länge des Preises ist ungültig")]
            string Price
    );
}
