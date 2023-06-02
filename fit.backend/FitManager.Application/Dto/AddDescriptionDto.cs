using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record AddDescriptionDto(
        Guid guid,
        string description
    );
}
