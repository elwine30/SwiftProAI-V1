using ThinknInsurTech.Companies;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Case;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Registration.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using Abp.Configuration;
using ThinknInsurTech.Common;
using ThinknInsurTech.Runtime;
using System.Collections.Specialized;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseInvoices)]
    public class CaseInvoicesAppService : ThinknInsurTechAppServiceBase, ICaseInvoicesAppService
    {
        private readonly IRepository<CaseInvoice> _caseInvoiceRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<CaseType, int> _lookup_caseTypeRepository;
        private readonly IRepository<CaseLawyer, int> _lookup_caseLawyerRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<CaseClaim> _caseClaimRepository;
        private readonly IRepository<CaseInsuredPerson> _insuredPersonRepository;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<DocumentSetting> _documentSettingRepository;

        public CaseInvoicesAppService(IRepository<CaseInvoice> caseInvoiceRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository, IRepository<User, long> lookup_userRepository, IRepository<CaseType, int> lookup_caseTypeRepository, IRepository<CaseLawyer, int> lookup_caseLawyerRepository, IAppConfigurationAccessor appConfigurationAccessor, IRepository<CaseClaim> caseClaimRepository, IRepository<CaseInsuredPerson> insuredPersonRepository, ISettingManager settingManager, IRepository<DocumentSetting> documentSettingRepository)
        {
            _caseInvoiceRepository = caseInvoiceRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_insuranceCompanyRepository = lookup_insuranceCompanyRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_caseTypeRepository = lookup_caseTypeRepository;
            _lookup_caseLawyerRepository = lookup_caseLawyerRepository;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _caseClaimRepository = caseClaimRepository;
            _insuredPersonRepository = insuredPersonRepository;
            _settingManager = settingManager;
            _documentSettingRepository = documentSettingRepository;

        }

        public virtual async Task<GetCaseInvoiceForPreviewDto> GetCaseInvoiceForPreview(int id)
        {
            var data = await _caseInvoiceRepository.GetAll().Select(s => new
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
                s.RegisterFk.StatusId,
                s.RegisterFk.InsuredPerson,
                CaseInvoice = s,
                s.RegisterFk.InvoiceItems,
            }).FirstOrDefaultAsync(w => w.RegisterId.Equals(id));


            var output = new GetCaseInvoiceForPreviewDto { CaseInvoice = ObjectMapper.Map<CaseInvoiceDto>(data.CaseInvoice) };
            var caseInvoice = data.CaseInvoice;

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

            if (caseInvoice != null)
            {
                var companyId = caseInvoice.CompanyId;
                var registerId = caseInvoice.RegisterId;
                output.MainRegistrationVehicleNo = data.VehicleNo;
                output.FileRefNo = data.FileRefNo;
                output.statusId = data.StatusId;

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

                if (data.InvoiceItems.Count > 0)
                {
                    output.InvoiceItems = new List<InvoiceItemDto>();
                    foreach (var item in data.InvoiceItems)
                    {
                        output.InvoiceItems.Add(new InvoiceItemDto()
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

        public virtual async Task<GetCaseInvoiceForViewDto> GetCaseInvoiceForView(int id)
        {
            var caseInvoice = await _caseInvoiceRepository.GetAsync(id);

            var output = new GetCaseInvoiceForViewDto { CaseInvoice = ObjectMapper.Map<CaseInvoiceDto>(caseInvoice) };

            if (output.CaseInvoice.RegisterId != null)
            {
                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseInvoice.RegisterId);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            }

            if (output.CaseInvoice.CompanyId != null)
            {
                var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)output.CaseInvoice.CompanyId);
                output.CompanyName = _lookupCompany?.Name?.ToString();
            }

            if (output.CaseInvoice.ClaimExecutive != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseInvoice.ClaimExecutive);
                output.ClaimExecutiveUserName = _lookupUser?.Name?.ToString();
            }

            if (output.CaseInvoice.AdjusterId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseInvoice.AdjusterId);
                output.AdjusterUserName = _lookupUser?.Name?.ToString();
            }

            if (output.CaseInvoice.CaseTypeId != null)
            {
                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.CaseInvoice.CaseTypeId);
                output.CaseTypeShortName = _lookupCaseType?.ShortName?.ToString();
            }

            return output;
        }


        [AbpAuthorize(AppPermissions.Pages_CaseInvoices_Edit)]
        public virtual async Task<GetCaseInvoiceForEditOutput> GetCaseInvoiceForEdit(EntityDto input)
        {
            var caseInvoice = await _caseInvoiceRepository.GetAll()
                .FirstOrDefaultAsync(w => w.RegisterId.Equals(input.Id));
            var caseClaim = await _caseClaimRepository.GetAll()
                .FirstOrDefaultAsync(w => w.RegisterId.Equals(input.Id));
            var mainRegistration = await _lookup_mainRegistrationRepository.GetAll()
                .FirstOrDefaultAsync(w => w.Id.Equals(input.Id));

            var output = new GetCaseInvoiceForEditOutput
            {
                CaseInvoice = ObjectMapper.Map<CreateOrEditCaseInvoiceDto>(caseInvoice),
                CaseClaim = ObjectMapper.Map<CaseClaimDto>(caseClaim)
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
                else {
                    output.PhotographCharge = decimal.Parse(charges["DefaultPhotographCharge"]);
                }
                output.CaseStatusId = mainRegistration.StatusId;

            }

            if (AbpSession.TenantId.HasValue)
            {
                output.TenantCompanyName = await _settingManager.GetSettingValueForTenantAsync("App.TenantManagement.BillingLegalName", AbpSession.TenantId.Value);
            }

            if (caseInvoice != null)
            {
                caseInvoice.MileageUnitPrice = decimal.Parse(charges["MileageUnitPrice"]);

                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseInvoice.RegisterId);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();

                if (caseInvoice.CompanyId != null)
                {
                    var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)output.CaseInvoice.CompanyId);
                    output.CompanyName = _lookupCompany?.Name?.ToString();
                }

                if (caseInvoice.ClaimExecutive != null)
                {
                    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseInvoice.ClaimExecutive);
                    output.ClaimExecutiveUserName = _lookupUser?.Name?.ToString();
                }

                var _lookupAdjusterUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CaseInvoice.AdjusterId);
                output.AdjusterUserName = _lookupAdjusterUser?.Name?.ToString();


                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.CaseInvoice.CaseTypeId);
                output.CaseTypeShortName = _lookupCaseType?.ShortName?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseInvoiceDto input)
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
            else {
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

        [AbpAuthorize(AppPermissions.Pages_CaseInvoices_Create)]
        protected virtual async Task Create(CreateOrEditCaseInvoiceDto input)
        {

            long? organizationUnitId = null;
            // Initialize default values
            var prefix = _appConfiguration["InvoiceRefNoSetting:Prefix"];
            var length = int.Parse(_appConfiguration["InvoiceRefNoSetting:InvoiceRefNoLength"]);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                organizationUnitId = AbpSession.GetCurrentOUId().Value;
                var documentSettings = await _documentSettingRepository.FirstOrDefaultAsync(p => p.OrganizationUnitId == organizationUnitId);
                if (documentSettings != null)
                {
                    // Update prefix and length if document settings exist
                    prefix = documentSettings.invoiceRefNoPrefix ?? prefix;
                    length = documentSettings.invoiceRefNoLength ?? length;
                }
            }

            IQueryable<int> query = _caseInvoiceRepository.GetAll()
                .Where(mr => mr.InvoiceRefNo != null)
                .Select(mr => Convert.ToInt32(mr.InvoiceRefNo));

            int currentMaxInvoiceRefNo = 0;

            if (query.Any())
                currentMaxInvoiceRefNo = query.Max();

            input.InvoiceRefNo = (currentMaxInvoiceRefNo + 1).ToString().PadLeft(length, '0');
            input.InvoiceRefNoPrefix = prefix;

            var caseInvoice = ObjectMapper.Map<CaseInvoice>(input);

            if (AbpSession.TenantId != null)
            {
                caseInvoice.TenantId = (int)AbpSession.TenantId;
            }
            caseInvoice.OrganizationUnitId = organizationUnitId;

            await _caseInvoiceRepository.InsertAsync(caseInvoice);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseInvoices_Edit)]
        protected virtual async Task Update(CreateOrEditCaseInvoiceDto input)
        {
            var caseInvoice = await _caseInvoiceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseInvoice);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseInvoices_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseInvoiceRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_CaseInvoices)]
        public async Task<List<CaseInvoiceMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CaseInvoiceMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }


        [AbpAuthorize(AppPermissions.Pages_CaseInvoices)]
        public async Task<List<CaseInvoiceUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new CaseInvoiceUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CaseInvoices)]
        public async Task<List<CaseInvoiceCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()
        {
            return await _lookup_caseTypeRepository.GetAll()
                .Select(caseType => new CaseInvoiceCaseTypeLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.ShortName == null ? "" : caseType.ShortName.ToString()
                }).ToListAsync();
        }

    }
}