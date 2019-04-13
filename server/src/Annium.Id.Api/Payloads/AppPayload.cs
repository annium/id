using System.ComponentModel.DataAnnotations;

namespace Annium.Id.Api.Payloads
{
    public class AppPayload
    {
        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(3)]
        public string Key { get; set; }

        [Required(ErrorMessage = Annotations.Required)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}