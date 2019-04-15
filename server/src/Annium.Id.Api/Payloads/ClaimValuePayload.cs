using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class ClaimValuePayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = Annotations.StringLength)]
        public string Value { get; set; }
    }
}