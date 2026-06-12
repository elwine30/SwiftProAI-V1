using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICaseDebitNotesAppService : IApplicationService
    {
        Task<GetCaseDebitNoteForPreviewDto> GetCaseDebitNoteForPreview(int id);

        Task<GetCaseDebitNoteForViewDto> GetCaseDebitNoteForView(int id);

        Task<GetCaseDebitNoteForEditOutput> GetCaseDebitNoteForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseDebitNoteDto input);

        Task Delete(EntityDto input);

        Task<List<CaseDebitNoteMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

        Task<List<CaseDebitNoteUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<CaseDebitNoteCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown();

    }
}