using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}