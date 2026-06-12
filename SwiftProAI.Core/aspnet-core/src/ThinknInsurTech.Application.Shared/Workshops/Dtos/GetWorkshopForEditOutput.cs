using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Workshops.Dtos
{
    public class GetWorkshopForEditOutput
    {
        public CreateOrEditWorkshopDto Workshop { get; set; }

    }
}