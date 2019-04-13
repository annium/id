using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class UserPayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(3)]
        [StringLength(50)]
        public string Login { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(8)]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(3)]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(3)]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(3)]
        [StringLength(100)]
        public string Email { get; set; }
    }
}