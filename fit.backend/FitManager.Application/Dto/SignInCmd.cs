using FitManager.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record SignInCmd
    (
        Guid guid,
        Guid packageGuid,
        Guid eventGuid
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // We have registered FitContext in Program.cs in ASP.NET core. So we can
            // get this service to access the database for further validation.
            var db = validationContext.GetRequiredService<FitContext>();
            if (!db.Packages.Any(a => a.Guid == packageGuid))
            {
                yield return new ValidationResult("Package existiert nicht", new[] { nameof(packageGuid) });
            }
            if (!db.Events.Any(c => c.Guid == eventGuid))
            {
                yield return new ValidationResult("Event existiert nicht", new[] { nameof(eventGuid) });
            }
        }
    };
}
