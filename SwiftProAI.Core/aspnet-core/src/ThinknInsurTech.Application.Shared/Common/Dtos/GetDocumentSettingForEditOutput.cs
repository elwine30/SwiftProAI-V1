using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetDocumentSettingForEditOutput
    {
        public CreateOrEditDocumentSettingDto DocumentSetting { get; set; }

    }
}