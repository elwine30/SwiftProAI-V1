using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICaseDeclarationAnswersAppService : IApplicationService
    {

        Task<GetCaseDeclarationAnswerForEditOutput> GetCaseDeclarationAnswerForEdit(EntityDto input);

        Task CreateOrEdit(List<CreateOrEditCaseDeclarationAnswerDto> input);


    }
}