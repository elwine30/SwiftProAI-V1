using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;
using ThinknInsurTech.Registration.Dto;

namespace ThinknInsurTech.Registration
{
    public interface ICaseExpensesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseExpenseForViewDto>> GetAll(GetAllCaseExpensesInput input);
        Task<GetCaseExpenseForViewDto> GetCaseExpenseForView(int id);
        Task Delete(EntityDto input);

        #region Adjuster Usage
        Task<GetCaseExpenseAdjusterViewDto> CreateExpenses(CreateExpenseInput input);
        Task UpdateCaseExpenses(UpdateCaseExpensesDTO input);
        Task<List<GetCaseExpenseAdjusterViewDto>> GetCaseExpenseAdjusterViewDto(int id);
        Task<List<CaseExpenseLookupTableDto>> GetExpensesTypeList(string lookupGroup);

        #endregion


    }
}