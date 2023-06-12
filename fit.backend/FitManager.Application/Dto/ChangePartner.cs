using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record ChangePartner
    (
        Guid Guid,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Vornamens ist ungültig")]
        string Firstname,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Nachnamens ist ungültig")]
        string Lastname,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Vornamens ist ungültig")]
        [EmailAddress]
        string Email,
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Die Länge der Telefonnummer ist ungültig")]
        string TelNr,
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Die Länge der Funktion ist ungültig")]
        string Function,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Titels ist ungültig")]
        string? Title = null,
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Die Länge der Mobil ist ungültig")]
        string? MobilNr = null,
        bool MainPartner = false
    );
}
