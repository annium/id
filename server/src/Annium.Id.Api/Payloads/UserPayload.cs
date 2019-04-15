using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class UserPayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Login { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = Annotations.StringLength)]
        public string Password { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string LastName { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Email { get; set; }
    }
}