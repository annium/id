using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class UserLoginPayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Login { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Password { get; set; }
    }
}