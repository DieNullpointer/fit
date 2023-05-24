using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Dto
{
    public record PartnerCmd
    (
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Titels ist ungültig")]
        string title,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Vornamens ist ungültig")]
        string firstname,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Nachnamens ist ungültig")]
        string lastname,
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Die Länge des Vornamens ist ungültig")]
        [EmailAddress]
        string email,
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Die Länge der Telefonnummer ist ungültig")]
        string telNr,
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Die Länge der Funktion ist ungültig")]
        string function,
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Die Länge der Mobil ist ungültig")]
        string? mobilNr = null,
        bool mainPartner = false
    );
}
