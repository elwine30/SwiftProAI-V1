using ThinknInsurTech.Registration;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Case;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Registration.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Abp.UI;
using ThinknInsurTech.Configuration;
using System;
using Abp.Configuration;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Common;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes)]
    public class CaseCreditNotesAppService : ThinknInsurTechAppServiceBase, ICaseCreditNotesAppService
    {
        private readonly IRepository<CaseCreditNote> _caseCreditNoteRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<CaseType, int> _lookup_caseTypeRepository;
        private readonly IRepository<CaseLawyer, int> _lookup_caseLawyerRepository;
        private readonly IRepository<CaseInsuredPerson> _insuredPersonRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<DocumentSetting, int> _documentSettingRepository;

        public CaseCreditNotesAppService(IRepository<CaseCreditNote> caseCreditNoteRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository, IRepository<User, long> lookup_userRepository, IRepository<CaseType, int> lookup_caseTypeRepository, IRepository<CaseLawyer, int> lookup_caseLawyerRepository, IAppConfigurationAccessor appConfigurationAccessor, IRepository<CaseInsuredPerson> insuredPersonRepository, IAppConfigurationAccessor configurationAccessor, ISettingManager settingManager, IRepository<DocumentSetting, int> documentSettingRepository)
        {
            _caseCreditNoteRepository = caseCreditNoteRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_insuranceCompanyRepository = lookup_insuranceCompanyRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_caseTypeRepository = lookup_caseTypeRepository;
            _lookup_caseLawyerRepository = lookup_caseLawyerRepository;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _insuredPersonRepository = insuredPersonRepository;
            _configurationAccessor = configurationAccessor;
            _settingManager = settingManager;
            _documentSettingRepository = documentSettingRepository;
        }

        public virtual async Task<GetCaseCreditNoteForPreviewDto> GetCaseCreditNoteForPreview(int id)
        {
            var data = await _caseCreditNoteRepository.GetAll().Select( s => new 
            {
                s.RegisterId,
                CaseCreditNote = s,
                CompanyFk = new
                {
                    s.CompanyFk.Name,
                    s.CompanyFk.Address,
                    s.CompanyFk.GstRegNo
                },
                s.RegisterFk.VehicleNo,
                s.RegisterFk.FileRefNo,
                s.RegisterFk.InsuredPerson,
                s.RegisterFk.CreditNoteItems
            }).FirstOrDefaultAsync(w => w.RegisterId.Equals(id));

            var output = new GetCaseCreditNoteForPreviewDto { CaseCreditNote = ObjectMapper.Map<CaseCreditNoteDto>(data.CaseCreditNote) };
            var caseCreditNote = data.CaseCreditNote;

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                var ouId = AbpSession.GetCurrentOUId().Value;
                var documentSettings = await _documentSettingRepository.FirstOrDefaultAsync(p => p.OrganizationUnitId == ouId);
                if (documentSettings != null)
                {
                    output.TenantCompanyName = documentSettings.companyLegalName;
                    output.TenantBusinessRegistrationNo = documentSettings.businessRegistrationNo;
                    output.TenantCompanyTaxVatNo = documentSettings.taxNo;
                    output.TenantCompanyAddress = documentSettings.address;
                    output.TenantCompanyTelNo = documentSettings.telNo;
                }
            }

            if (data != null)
            {
                var companyId = data.CaseCreditNote.CompanyId;
                output.MainRegistrationVehicleNo = data.VehicleNo;
                output.FileRefNo = data.FileRefNo;

                if (data.CompanyFk != null)
                {
                    output.CompanyName = data.CompanyFk.Name;
                    output.CompanyAddress = data.CompanyFk.Address;
                    output.CompanySstRegNo = data.CompanyFk.GstRegNo;
                }

                var _lookupInsuredPerson = data.InsuredPerson.Where(w => w.IsOwner).FirstOrDefault();
                if (_lookupInsuredPerson != null)
                {
                    output.InsuredPersonName = _lookupInsuredPerson.Name;
                    output.InsuredPersonPolicyNo = _lookupInsuredPerson.PolicyNo;
                }

                if (data.CreditNoteItems.Count > 0)
                {
                    output.CreditNoteItems = new List<CreditNoteItemDto>();
                    foreach (var item in data.CreditNoteItems)
                    {
                        output.CreditNoteItems.Add(new CreditNoteItemDto()
                        {
                            Amount = item.Amount,
                            ItemType = item.ItemType,
                            Remark = item.Remark
                        });
                    }
                }
            }

            return output;
        }

        public virtual async Task<GetCaseCreditNoteForViewDto> GetCaseCreditNoteForView(int id)
        {
            var caseCreditNote = await _caseCreditNoteRepository.GetAsync(id);

            var output = new GetCaseCreditNoteForViewDto { CaseCreditNote = ObjectMapper.Map<CaseCreditNoteDto>(caseCreditNote) };

            if (output.CaseCreditNote.RegisterId != null)
            {
                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseCreditNote.RegisterId);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            }

            if (output.CaseCreditNote.CompanyId != null)
            {
                var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)output.CaseCreditNote.CompanyId);
                output.CompanyName = _lookupCompany?.Name?.ToString();
            }

            if (output.CaseCreditNote.ClaimExecutive != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseCreditNote.ClaimExecutive);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.CaseCreditNote.AdjusterId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseCreditNote.AdjusterId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

            if (output.CaseCreditNote.CaseTypeId != null)
            {
                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.CaseCreditNote.CaseTypeId);
                output.CaseTypeDescription = _lookupCaseType?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes_Edit)]
        public virtual async Task<GetCaseCreditNoteForEditOutput> GetCaseCreditNoteForEdit(EntityDto input)
        {
            var caseCreditNote = await _caseCreditNoteRepository
                .FirstOrDefaultAsync(w => w.RegisterId.Equals(input.Id));
            var mainRegistration = await _lookup_mainRegistrationRepository
                .FirstOrDefaultAsync(w => w.Id.Equals(input.Id));

            var output = new GetCaseCreditNoteForEditOutput { 
                CaseCreditNote = ObjectMapper.Map<CreateOrEditCaseCreditNoteDto>(caseCreditNote) 
            };

            var charges = _appConfiguration.GetSection("ChargeRates");
            output.MileageUnitPrice = decimal.Parse(charges["MileageUnitPrice"]);
            output.SSTRate = decimal.Parse(charges["SSTRate"]);

            if (mainRegistration != null)
            {
                var company = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync(w => w.Id.Equals(mainRegistration.CompanyId));
                if (company != null)
                {
                    output.PhotographCharge = company?.PhotographCharge ?? decimal.Parse(charges["DefaultPhotographCharge"]);
                }
                else
                {
                    output.PhotographCharge = decimal.Parse(charges["DefaultPhotographCharge"]);
                }
                output.CaseStatusId = mainRegistration.StatusId;

            }

            if (AbpSession.TenantId.HasValue)
            {
                output.TenantCompanyName = await _settingManager.GetSettingValueForTenantAsync("App.TenantManagement.BillingLegalName", AbpSession.TenantId.Value);
            }

            if (caseCreditNote != null)
            {
                caseCreditNote.MileageUnitPrice = decimal.Parse(charges["MileageUnitPrice"]);

                if (output.CaseCreditNote.RegisterId != null)
                {
                    var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseCreditNote.RegisterId);
                    output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
                }

                if (output.CaseCreditNote.CompanyId != null)
                {
                    var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)output.CaseCreditNote.CompanyId);
                    output.CompanyName = _lookupCompany?.Name?.ToString();
                }

                if (output.CaseCreditNote.ClaimExecutive != null)
                {
                    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseCreditNote.ClaimExecutive);
                    output.ClaimExecutiveUserName = _lookupUser?.Name?.ToString();
                }

                if (output.CaseCreditNote.AdjusterId != null)
                {
                    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseCreditNote.AdjusterId);
                    output.AdjusterUserName = _lookupUser?.Name?.ToString();
                }

                if (output.CaseCreditNote.CaseTypeId != null)
                {
                    var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.CaseCreditNote.CaseTypeId);
                    output.CaseTypeShortName = _lookupCaseType?.Description?.ToString();
                }
            }
            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseCreditNoteDto input)
        {
            var mainRegistration = await _lookup_mainRegistrationRepository.GetAll()
            .FirstOrDefaultAsync(w => w.Id.Equals(input.RegisterId));

            input.CaseTypeId = mainRegistration?.CaseTypeId;
            input.CompanyId = mainRegistration.CompanyId;

            var adjusterUser = await _lookup_userRepository.FirstOrDefaultAsync((long)mainRegistration.AdjusterMemberId);
            if (adjusterUser != null)
            {
                input.AdjusterId = mainRegistration.AdjusterMemberId;
            }
            else
            {
                throw new UserFriendlyException($"Error: Adjuster Member with Id {mainRegistration.AdjusterMemberId} is not found. Please reassign the case to an existing adjuster.");
            }

            var caseLawyer = await _lookup_caseLawyerRepository.FirstOrDefaultAsync(w => w.RegisterId.Equals(input.RegisterId));
            if (caseLawyer != null)
            {
                input.Lawyer = caseLawyer.ContactName;
            }

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes_Create)]
        protected virtual async Task Create(CreateOrEditCaseCreditNoteDto input)
        {
            long? organizationUnitId = null;

            // Initialize default values
            var prefix = _configurationAccessor.Configuration["CreditRefNoSetting:Prefix"];
            var length = Convert.ToInt32(_configurationAccessor.Configuration["CreditRefNoSetting:CreditRefNoLength"]);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                organizationUnitId = AbpSession.GetCurrentOUId().Value;
                var documentSettings = await _documentSettingRepository.FirstOrDefaultAsync(p => p.OrganizationUnitId == organizationUnitId);
                if (documentSettings != null)
                {
                    // Update prefix and length if document settings exist
                    prefix = documentSettings.creditRefNoPrefix ?? prefix;
                    length = documentSettings.creditRefNoLength ?? length;
                }

            }

            IQueryable<int> query = _caseCreditNoteRepository.GetAll()
                .Where(mr => mr.CreditRefNo != null)
                .Select(mr => Convert.ToInt32(mr.CreditRefNo));

            int currentMaxCreditNoteRefNo = 0;

            if (query.Any())
                currentMaxCreditNoteRefNo = query.Max();

            input.CreditRefNo = (currentMaxCreditNoteRefNo + 1).ToString().PadLeft(length, '0');
            input.CreditRefNoPrefix = prefix;
            var caseCreditNote = ObjectMapper.Map<CaseCreditNote>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseCreditNote.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseCreditNote.TenantId = (int)AbpSession.TenantId;
            }

            await _caseCreditNoteRepository.InsertAsync(caseCreditNote);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes_Edit)]
        protected virtual async Task Update(CreateOrEditCaseCreditNoteDto input)
        {
            var caseCreditNote = await _caseCreditNoteRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseCreditNote);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseCreditNoteRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes)]
        public async Task<List<CaseCreditNoteMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CaseCreditNoteMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }


        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes)]
        public async Task<List<CaseCreditNoteUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new CaseCreditNoteUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CaseCreditNotes)]
        public async Task<List<CaseCreditNoteCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()  
        {
            return await _lookup_caseTypeRepository.GetAll()
                .Select(caseType => new CaseCreditNoteCaseTypeLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.Description == null ? "" : caseType.Description.ToString()
                }).ToListAsync();
        }

    }
}