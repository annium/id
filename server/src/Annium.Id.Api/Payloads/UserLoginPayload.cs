using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class UserLoginPayload
    {
        [Required]
        [MinLength(3)]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        [MinLength(8)]
        [StringLength(50)]
        public string Password { get; set; }
    }
}