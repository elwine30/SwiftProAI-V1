using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface IDebitNoteItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDebitNoteItemForViewDto>> GetAll(GetAllDebitNoteItemsInput input);

        Task<List<DebitNoteItemDto>> GetDebitNoteItemByRegisterId(EntityDto input);

        Task<List<DebitNoteItemAmountDto>> GetDebitNoteItemAmountsByRegisterId(EntityDto input);

        Task<GetDebitNoteItemForEditOutput> GetDebitNoteItemForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDebitNoteItemDto input);

        Task Delete(EntityDto input);

        Task<List<DebitNoteItemMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

    }
}