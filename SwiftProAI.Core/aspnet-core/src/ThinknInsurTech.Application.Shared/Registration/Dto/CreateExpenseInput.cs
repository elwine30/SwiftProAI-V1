using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThinknInsurTech.Registration.Dto
{
    public class CreateExpenseInput
    {
        [Required]
        public double Amount { get; set; }

        [MaxLength(255)]
        public string Remark { get; set; }

        [Required]
        public int TypeId { get; set; }

        public int? SubTypeId { get; set; } // nullable

        [Required]
        public int MainRegistrationId { get; set; }

    }
}

