using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICaseCreditNotesAppService : IApplicationService
    {
        Task<GetCaseCreditNoteForPreviewDto> GetCaseCreditNoteForPreview(int id);
        Task<GetCaseCreditNoteForViewDto> GetCaseCreditNoteForView(int id);

        Task<GetCaseCreditNoteForEditOutput> GetCaseCreditNoteForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseCreditNoteDto input);

        Task Delete(EntityDto input);

        Task<List<CaseCreditNoteMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

        Task<List<CaseCreditNoteUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<CaseCreditNoteCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown();

    }
}