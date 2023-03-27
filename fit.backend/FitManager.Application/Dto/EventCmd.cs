using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record EventCmd
    (
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Die Länge des Namens ist ungültig.")]
            string name,
            DateTime date
    );
}
