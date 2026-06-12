using System;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dto
{
    // This DTO is to be catered for InsuredPersons's related files
    public class RemoveFile
    {
        [Required]
        public Guid FileToken { get; set; }
        //TODO : To follow the right procedure is, when users upload need save the guid into insured's person field 
        //Current implementatin
        //[Required]
        //public int? ThirdPartyInfoId { get; set; }
        //[Required]
        //public string TypeFile { get; set; }


    }
}

