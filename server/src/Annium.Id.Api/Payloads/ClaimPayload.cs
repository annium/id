using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class ClaimPayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Key { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Name { get; set; }
    }
}