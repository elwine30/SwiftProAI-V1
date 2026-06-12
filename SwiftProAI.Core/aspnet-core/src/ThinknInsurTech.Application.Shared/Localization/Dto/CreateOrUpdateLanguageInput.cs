using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}