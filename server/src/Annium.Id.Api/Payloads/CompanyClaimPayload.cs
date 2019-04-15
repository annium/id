using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class CompanyClaimPayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3)]
        public string Key { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
    }
}