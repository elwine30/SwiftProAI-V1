using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
