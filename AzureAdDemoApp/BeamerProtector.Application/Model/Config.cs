using System.ComponentModel.DataAnnotations;

namespace BeamerProtector.Application.Model
{
    public class Config
    {
        public int Id { get; set; }
        public string? MailerAccountname { get; set; }

        [StringLength(4096)]
        public string? MailerRefreshToken { get; set; }
    }
}