using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface IExpensesClaimsApproval
    {
        Task<PagedResultDto<GetExpensesApprovalForViewDTO>> GetAllExpensesApproval(GetExpensesClaimsApprovalInput input);
        Task<PagedResultDto<GetClaimsApprovalForViewDTO>> GetAllClaimsForApproval(GetExpensesClaimsApprovalInput input);
        Task UpdateExpenses(List<ExpensesClaimsApprovalDto> dtoList);
        Task UpdateClaims(List<ExpensesClaimsApprovalDto> dtoList);
    }
}
