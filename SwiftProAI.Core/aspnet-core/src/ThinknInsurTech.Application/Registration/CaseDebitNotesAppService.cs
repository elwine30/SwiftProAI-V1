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
using ThinknInsurTech.Common;
using ThinknInsurTech.Runtime;


namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes)]
    public class CaseDebitNotesAppService : ThinknInsurTechAppServiceBase, ICaseDebitNotesAppService
    {
        private readonly IRepository<CaseDebitNote> _caseDebitNoteRepository;
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


        public CaseDebitNotesAppService(IRepository<CaseDebitNote> caseDebitNoteRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository, IRepository<User, long> lookup_userRepository, IRepository<CaseType, int> lookup_caseTypeRepository, IRepository<CaseLawyer, int> lookup_caseLawyerRepository, IAppConfigurationAccessor appConfigurationAccessor, IRepository<CaseInsuredPerson> insuredPersonRepository, IAppConfigurationAccessor configurationAccessor, ISettingManager settingManager, IRepository<DocumentSetting, int> documentSettingRepository)
        {
            _caseDebitNoteRepository = caseDebitNoteRepository;
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

        public virtual async Task<GetCaseDebitNoteForPreviewDto> GetCaseDebitNoteForPreview(int id)
        {
            var data = await _caseDebitNoteRepository.GetAll().Select(s => new
            {
                s.RegisterId,
                CompanyFk = new
                {
                    s.CompanyFk.Name,
                    s.CompanyFk.Address,
                    s.CompanyFk.GstRegNo
                },
                s.RegisterFk.VehicleNo,
                s.RegisterFk.FileRefNo,
                s.RegisterFk.InsuredPerson,
                CaseDebitNote = s,
                s.RegisterFk.DebitNoteItems,
            }).FirstOrDefaultAsync(w => w.RegisterId.Equals(id));


            var output = new GetCaseDebitNoteForPreviewDto { CaseDebitNote = ObjectMapper.Map<CaseDebitNoteDto>(data.CaseDebitNote) };
            var caseDebitNote = data.CaseDebitNote;

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

            if (caseDebitNote != null)
            {
                var companyId = caseDebitNote.CompanyId;
                var registerId = caseDebitNote.RegisterId;
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

                if (data.DebitNoteItems.Count > 0)
                {
                    output.DebitNoteItems = new List<DebitNoteItemDto>();
                    foreach (var item in data.DebitNoteItems)
                    {
                        output.DebitNoteItems.Add(new DebitNoteItemDto()
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

        public virtual async Task<GetCaseDebitNoteForViewDto> GetCaseDebitNoteForView(int id)
        {
            var caseDebitNote = await _caseDebitNoteRepository.GetAsync(id);

            var output = new GetCaseDebitNoteForViewDto { CaseDebitNote = ObjectMapper.Map<CaseDebitNoteDto>(caseDebitNote) };

            if (output.CaseDebitNote.RegisterId != null)
            {
                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseDebitNote.RegisterId);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            }

            if (output.CaseDebitNote.CompanyId != null)
            {
                var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)output.CaseDebitNote.CompanyId);
                output.CompanyName = _lookupCompany?.Name?.ToString();
            }

            if (output.CaseDebitNote.ClaimExecutive != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseDebitNote.ClaimExecutive);
                output.ClaimExecutiveUserName = _lookupUser?.Name?.ToString();
            }

            if (output.CaseDebitNote.AdjusterId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseDebitNote.AdjusterId);
                output.AdjusterUserName = _lookupUser?.Name?.ToString();
            }

            if (output.CaseDebitNote.CaseTypeId != null)
            {
                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.CaseDebitNote.CaseTypeId);
                output.CaseTypeShortName = _lookupCaseType?.ShortName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes_Edit)]
        public virtual async Task<GetCaseDebitNoteForEditOutput> GetCaseDebitNoteForEdit(EntityDto input)
        {
            var caseDebitNote = await _caseDebitNoteRepository
                .FirstOrDefaultAsync(w => w.RegisterId.Equals(input.Id));
            var mainRegistration = await _lookup_mainRegistrationRepository
                .FirstOrDefaultAsync(w => w.Id.Equals(input.Id));

            var output = new GetCaseDebitNoteForEditOutput { 
                CaseDebitNote = ObjectMapper.Map<CreateOrEditCaseDebitNoteDto>(caseDebitNote) 
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

            if (caseDebitNote != null)
            {
                caseDebitNote.MileageUnitPrice = decimal.Parse(charges["MileageUnitPrice"]);

                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseDebitNote.RegisterId);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();

                if (caseDebitNote.CompanyId != null)
                {
                    var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)output.CaseDebitNote.CompanyId);
                    output.CompanyName = _lookupCompany?.Name?.ToString();
                }

                if (caseDebitNote.ClaimExecutive != null)
                {
                    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseDebitNote.ClaimExecutive);
                    output.ClaimExecutiveUserName = _lookupUser?.Name?.ToString();
                }

                var _lookupAdjusterUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseDebitNote.AdjusterId);
                output.AdjusterUserName = _lookupAdjusterUser?.Name?.ToString();


                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.CaseDebitNote.CaseTypeId);
                output.CaseTypeShortName = _lookupCaseType?.ShortName?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseDebitNoteDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes_Create)]
        protected virtual async Task Create(CreateOrEditCaseDebitNoteDto input)
        {

            long? organizationUnitId = null;

            // Initialize default values
            var prefix = _configurationAccessor.Configuration["DebitRefNoSetting:Prefix"];
            var length = Convert.ToInt32(_configurationAccessor.Configuration["DebitRefNoSetting:DebitRefNoLength"]);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                organizationUnitId = AbpSession.GetCurrentOUId().Value;
                var documentSettings = await _documentSettingRepository.FirstOrDefaultAsync(p => p.OrganizationUnitId == organizationUnitId);
                if (documentSettings != null)
                {
                    // Update prefix and length if document settings exist
                    prefix = documentSettings.debitRefNoPrefix ?? prefix;
                    length = documentSettings.debitRefNoLength ?? length;
                }

            }

            IQueryable<int> query = _caseDebitNoteRepository.GetAll()
                .Where(mr => mr.DebitRefNo != null)
                .Select(mr => Convert.ToInt32(mr.DebitRefNo));

            int currentMaxDebitNoteRefNo = 0;

            if (query.Any())
                currentMaxDebitNoteRefNo = query.Max();

            input.DebitRefNo = (currentMaxDebitNoteRefNo + 1).ToString().PadLeft(length, '0');
            input.DebitRefNoPrefix = prefix;

            var caseDebitNote = ObjectMapper.Map<CaseDebitNote>(input);

            if (AbpSession.TenantId != null)
            {
                caseDebitNote.TenantId = (int)AbpSession.TenantId;
            }

            caseDebitNote.OrganizationUnitId = organizationUnitId;

            await _caseDebitNoteRepository.InsertAsync(caseDebitNote);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes_Edit)]
        protected virtual async Task Update(CreateOrEditCaseDebitNoteDto input)
        {
            var caseDebitNote = await _caseDebitNoteRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseDebitNote);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseDebitNoteRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes)]
        public async Task<List<CaseDebitNoteMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CaseDebitNoteMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }


        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes)]
        public async Task<List<CaseDebitNoteUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new CaseDebitNoteUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CaseDebitNotes)]
        public async Task<List<CaseDebitNoteCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()
        {
            return await _lookup_caseTypeRepository.GetAll()
                .Select(caseType => new CaseDebitNoteCaseTypeLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.ShortName == null ? "" : caseType.ShortName.ToString()
                }).ToListAsync();
        }

    }
}