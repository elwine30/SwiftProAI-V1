using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICreditNoteItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCreditNoteItemForViewDto>> GetAll(GetAllCreditNoteItemsInput input);

        Task<GetCreditNoteItemForEditOutput> GetCreditNoteItemForEdit(EntityDto input);

        Task BulkCreate(int registerId);

        Task CreateOrEdit(CreateOrEditCreditNoteItemDto input);

        Task Delete(EntityDto input);

        Task<List<CreditNoteItemAmountDto>> GetCreditNoteItemAmountsByRegisterId(EntityDto input);

        Task<List<CreditNoteItemMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

    }
}