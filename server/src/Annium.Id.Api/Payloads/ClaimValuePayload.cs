using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class ClaimValuePayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3)]
        public string Value { get; set; }
    }
}