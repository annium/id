using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class AppPayload
    {
        [Required]
        [MinLength(3)]
        public string Login { get; set; }

        [Required]
        [MinLength(10)]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}