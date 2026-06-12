using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseClaimsAppService : IApplicationService
    {

        Task<GetCaseClaimForEditOutput> GetCaseClaimForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseClaimDto input);

        Task<PagedResultDto<CreateOrEditCaseClaimDto>> GetAll(CaseClaimMainRegistrationInput input);

    }
}