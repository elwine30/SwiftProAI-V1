using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_InsuredPersons)]
    public class InsuredPersonsAppService : ThinknInsurTechAppServiceBase, IInsuredPersonsAppService
    {
        private readonly IRepository<CaseInsuredPerson> _insuredPersonRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<Hospital, int> _lookup_hospitalRepository;
        private readonly IRepository<Location, int> _lookup_locationRepository;
        private readonly IRepository<CasePoliceReport> _casePoliceReportRepository;
        private readonly IRepository<CasePoliceReportSummary, int> _casePoliceReportSummaryRepository;

        private readonly IFolderService _folderService;
        private readonly IFileOrgService _fileOrgService;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public InsuredPersonsAppService(
            IRepository<CaseInsuredPerson> insuredPersonRepository,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository,
            IRepository<Hospital, int> lookup_hospitalRepository,
            IRepository<Location, int> lookup_locationRepository,
            IRepository<FileOrg> fileOrgRepository,
                        IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository,
            IRepository<CasePoliceReport> casePoliceReportRepository,
            IRepository<CasePoliceReportSummary, int> casePoliceReportSummaryRepository,

            IFolderService folderService,
            IFileOrgService fileOrgService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _insuredPersonRepository = insuredPersonRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_hospitalRepository = lookup_hospitalRepository;
            _lookup_locationRepository = lookup_locationRepository;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
            _casePoliceReportRepository = casePoliceReportRepository;
            _casePoliceReportSummaryRepository = casePoliceReportSummaryRepository;

            _folderService = folderService;
            _fileOrgService = fileOrgService;

        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task<GetInsuredPersonForEditOutput> GetInsuredPersonForEdit(EntityDto input, bool isOwner)
        {
            var insuredPerson = _insuredPersonRepository.GetAll().Where(w => w.RegisterId.Equals(input.Id))
                                    .WhereIf(isOwner, e => e.IsOwner)
                                    .WhereIf(isOwner == false, e => e.IsDriver)
                                    .FirstOrDefault();

            var output = new GetInsuredPersonForEditOutput { InsuredPerson = ObjectMapper.Map<CreateOrEditInsuredPersonDto>(insuredPerson) };

            var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);
            output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            //}

            if (output.InsuredPerson != null)
            {
                if (output.InsuredPerson.HospitalId != null)
                {
                    var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.InsuredPerson.HospitalId);
                    output.HospitalName = _lookupHospital?.Name?.ToString();
                    output.HospitalAddress = _lookupHospital?.Address?.ToString();
                }
                if (output.InsuredPerson.CountryLocationId != null)
                {
                    var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.InsuredPerson.CountryLocationId);
                    output.LocationName = _lookupLocation?.Name?.ToString();

                    if (output.InsuredPerson.StateLocationId != null)
                    {
                        _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.InsuredPerson.StateLocationId);
                        output.LocationName2 = _lookupLocation?.Name?.ToString();
                    }
                }

                if (output.InsuredPerson.DriverICFront != null)
                    output.DriverICFrontFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverICFront);

                if (output.InsuredPerson.DriverICBack != null)
                    output.DriverICBackFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverICBack);

                if (output.InsuredPerson.DriverLicenseFront != null)
                    output.DriverLicenseFrontFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverLicenseFront);

                if (output.InsuredPerson.DriverLicenseBack != null)
                    output.DriverLicenseBackFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverLicenseBack);

                if (output.InsuredPerson.DriverEmploymentDetail != null)
                    output.DriverEmploymentDetailFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverEmploymentDetail);

                if (output.InsuredPerson.DriverHospitalDetail != null)
                    output.DriverHospitalDetailFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverHospitalDetail);

                if (output.InsuredPerson.DriverCarGrant != null)
                    output.DriverCarGrantFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverCarGrant);
            }
            return output;
        }

        public async Task<GetInsuredPersonForViewDto> GetInsuredPersonForView(EntityDto input, bool isOwner)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;
                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == input.Id).Select(f => f.RegisterId).FirstOrDefault();

                var insuredPerson = _insuredPersonRepository.GetAll().Where(w => w.RegisterId == assignedRegisterId)
                                   .WhereIf(isOwner, e => e.IsOwner)
                                   .WhereIf(isOwner == false, e => e.IsDriver)
                                   .FirstOrDefault();

                var output = new GetInsuredPersonForViewDto { InsuredPerson = ObjectMapper.Map<InsuredPersonDto>(insuredPerson) };
                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();

                if (output.InsuredPerson != null)
                {
                    if (output.InsuredPerson.HospitalId != null)
                    {
                        var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.InsuredPerson.HospitalId);
                        output.HospitalName = _lookupHospital?.Name?.ToString();
                        output.HospitalAddress = _lookupHospital?.Address?.ToString();
                    }
                    if (output.InsuredPerson.CountryLocationId != null)
                    {
                        var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.InsuredPerson.CountryLocationId);
                        output.LocationName = _lookupLocation?.Name?.ToString();

                        if (output.InsuredPerson.StateLocationId != null)
                        {
                            _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.InsuredPerson.StateLocationId);
                            output.LocationName2 = _lookupLocation?.Name?.ToString();
                        }
                    }


                    if (output.InsuredPerson.DriverICFront != null)
                        output.InsuredPerson.DriverICFrontFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverICFront);

                    if (output.InsuredPerson.DriverICBack != null)
                        output.InsuredPerson.DriverICBackFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverICBack);

                    if (output.InsuredPerson.DriverLicenseFront != null)
                        output.InsuredPerson.DriverLicenseFrontFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverLicenseFront);

                    if (output.InsuredPerson.DriverLicenseBack != null)
                        output.InsuredPerson.DriverLicenseBackFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverLicenseBack);

                    if (output.InsuredPerson.DriverEmploymentDetail != null)
                        output.InsuredPerson.DriverEmploymentDetailFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverEmploymentDetail);

                    if (output.InsuredPerson.DriverHospitalDetail != null)
                        output.InsuredPerson.DriverHospitalDetailFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverHospitalDetail);

                    if (output.InsuredPerson.DriverCarGrant != null)
                        output.InsuredPerson.DriverCarGrantFileName = await _fileOrgService.GetBinaryFileName(insuredPerson.DriverCarGrant);
                }
                return output;
            }


        }

        public virtual async Task<bool?> CreateOrEdit(CreateOrEditInsuredPersonDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
                return null;
            }
            else
            {
                return await Update(input);
            }
        }

        public virtual async Task<bool?> CreateOrEditInsuredDriver(CreateOrEditInsuredPersonDto input)
        {

            var insuredPerson = _insuredPersonRepository.GetAll()
                                    .Where(w => w.RegisterId.Equals(input.RegisterId))
                                    .Where(w => w.IsDriver)
                                    .FirstOrDefault();

            if (insuredPerson == null)
            {
                await Create(input);
                return null;
            }
            else
            {
                input.Id = insuredPerson.Id;
                return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Create)]
        protected virtual async Task Create(CreateOrEditInsuredPersonDto input)
        {
            if (!ValidateLicenseDateRange(input.LicenseDateFrom, input.LicenseDateTo))
            {
                throw new UserFriendlyException("License Date To must be greater than License Date From");

            }
            var insuredPerson = ObjectMapper.Map<CaseInsuredPerson>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                insuredPerson.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                insuredPerson.TenantId = (int)AbpSession.TenantId;
            }

            // IC checking with casePoliceReport before update
            // Check only if the IC is different
            var inputIdentityNo = Regex.Replace(input.IdenticationNo, @"[\s-]", "");

            var cprList = await _casePoliceReportRepository.GetAll()
                .Where(x => x.ReportType == "Claimant" && x.RegisterId == input.RegisterId)
                .ToListAsync();

            foreach (var report in cprList)
            {
                var reportIdentityNo = Regex.Replace(report.ComplainantIdentityNo, @"[\s-]", "");

                if (reportIdentityNo == inputIdentityNo && !report.IsDataConsistent)
                {
                    report.IsDataConsistent = true;
                    await _casePoliceReportRepository.UpdateAsync(report);
                }
            }

            await _insuredPersonRepository.InsertAsync(insuredPerson);

            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if (input.IsDriver)
                {
                    insuredPerson.DriverICFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICFrontToken, input.RegisterId, EnumFolderField.DriverICFront);
                    insuredPerson.DriverICBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICBackToken, input.RegisterId, EnumFolderField.DriverICBack);
                    insuredPerson.DriverLicenseFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseFrontToken, input.RegisterId, EnumFolderField.DriverDrivingLicenseFront);
                    insuredPerson.DriverLicenseBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseBackToken, input.RegisterId, EnumFolderField.DriverDrivingLicenseBack);
                    insuredPerson.DriverEmploymentDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverEmploymentDetailToken, input.RegisterId, EnumFolderField.DriverEmploymentDoc);
                    insuredPerson.DriverHospitalDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverHospitalDetailToken, input.RegisterId, EnumFolderField.DriverHospitalDoc);
                    insuredPerson.DriverCarGrant = await _fileOrgService.GetBinaryObjectFromCache(input.DriverCarGrantToken, input.RegisterId, EnumFolderField.DriverCarGrant);
                }
                else if (input.IsOwner)
                {
                    insuredPerson.DriverICFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICFrontToken, input.RegisterId, EnumFolderField.InsuredOwnerICFront);
                    insuredPerson.DriverICBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICBackToken, input.RegisterId, EnumFolderField.InsuredOwnerICBack);
                    insuredPerson.DriverLicenseFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseFrontToken, input.RegisterId, EnumFolderField.InsuredOwnerDrivingLicenseFront);
                    insuredPerson.DriverLicenseBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseBackToken, input.RegisterId, EnumFolderField.InsuredOwnerDrivingLicenseBack);
                    insuredPerson.DriverEmploymentDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverEmploymentDetailToken, input.RegisterId, EnumFolderField.InsuredOwnerEmploymentDoc);
                    insuredPerson.DriverHospitalDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverHospitalDetailToken, input.RegisterId, EnumFolderField.InsuredOwnerHospitalDoc);
                    insuredPerson.DriverCarGrant = await _fileOrgService.GetBinaryObjectFromCache(input.DriverCarGrantToken, input.RegisterId, EnumFolderField.InsuredOwnerCarGrant);
                }
            }


        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        protected virtual async Task<bool> Update(CreateOrEditInsuredPersonDto input)
        {
            var showInconsistencyError = false;
            if (!ValidateLicenseDateRange(input.LicenseDateFrom, input.LicenseDateTo))
            {
                throw new UserFriendlyException("License Date To must be greater than License Date From");

            }
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync((int)input.Id);

            // IC checking with casePoliceReport before update
            // Check only if the IC is different
            if (insuredPerson.IdenticationNo != input.IdenticationNo)
            {
                var inputIdentityNo = Regex.Replace(input.IdenticationNo, @"[\s-]", "");
                var prevInsPersonIdentityNo = insuredPerson.IdenticationNo != null ? Regex.Replace(insuredPerson.IdenticationNo, @"[\s-]", "") : null;

                var cprList = await _casePoliceReportRepository.GetAll()
                    .Where(x => x.ReportType == "Claimant" && x.RegisterId == input.RegisterId)
                    .ToListAsync(); // checking with BA if adjuster can upload only one or multiple claimant reports

                foreach (var report in cprList)
                {
                    var reportIdentityNo = Regex.Replace(report.ComplainantIdentityNo, @"[\s-]", "");

                    if (reportIdentityNo == inputIdentityNo && !report.IsDataConsistent)
                    {
                        report.IsDataConsistent = true;
                        await _casePoliceReportRepository.UpdateAsync(report);
                    }

                    if (prevInsPersonIdentityNo != null && reportIdentityNo == prevInsPersonIdentityNo && report.IsDataConsistent)
                    {
                        showInconsistencyError = true;
                        report.IsDataConsistent = false;
                        await _casePoliceReportRepository.UpdateAsync(report);

                        var casePoliceReportSummaryList = await _casePoliceReportSummaryRepository.GetAll()
                            .Where(x => x.RegisterId.Equals(input.RegisterId))
                            .ToListAsync();

                        if (casePoliceReportSummaryList.Count != 0)
                        {
                            foreach (var casePoliceReportSummary in casePoliceReportSummaryList)
                            {
                                await _casePoliceReportSummaryRepository.DeleteAsync(casePoliceReportSummary.Id);
                            }
                        }
                    }
                }
            }

            ObjectMapper.Map(input, insuredPerson);

            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if (input.IsDriver)
                {
                    if (!input.DriverICFrontToken.IsNullOrEmpty())
                        insuredPerson.DriverICFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICFrontToken, input.RegisterId, EnumFolderField.DriverICFront);
                    if (!input.DriverICBackToken.IsNullOrEmpty())
                        insuredPerson.DriverICBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICBackToken, input.RegisterId, EnumFolderField.DriverICBack);
                    if (!input.DriverLicenseFrontToken.IsNullOrEmpty())
                        insuredPerson.DriverLicenseFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseFrontToken, input.RegisterId, EnumFolderField.DriverDrivingLicenseFront);
                    if (!input.DriverLicenseBackToken.IsNullOrEmpty())
                        insuredPerson.DriverLicenseBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseBackToken, input.RegisterId, EnumFolderField.DriverDrivingLicenseBack);
                    if (!input.DriverEmploymentDetailToken.IsNullOrEmpty())
                        insuredPerson.DriverEmploymentDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverEmploymentDetailToken, input.RegisterId, EnumFolderField.DriverEmploymentDoc);
                    if (!input.DriverHospitalDetailToken.IsNullOrEmpty())
                        insuredPerson.DriverHospitalDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverHospitalDetailToken, input.RegisterId, EnumFolderField.DriverHospitalDoc);
                    if (!input.DriverCarGrantToken.IsNullOrEmpty())
                        insuredPerson.DriverCarGrant = await _fileOrgService.GetBinaryObjectFromCache(input.DriverCarGrantToken, input.RegisterId, EnumFolderField.DriverCarGrant);
                }
                else if (input.IsOwner)
                {
                    if (!input.DriverICFrontToken.IsNullOrEmpty())
                        insuredPerson.DriverICFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICFrontToken, input.RegisterId, EnumFolderField.InsuredOwnerICFront);
                    if (!input.DriverICBackToken.IsNullOrEmpty())
                        insuredPerson.DriverICBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverICBackToken, input.RegisterId, EnumFolderField.InsuredOwnerICBack);
                    if (!input.DriverLicenseFrontToken.IsNullOrEmpty())
                        insuredPerson.DriverLicenseFront = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseFrontToken, input.RegisterId, EnumFolderField.InsuredOwnerDrivingLicenseFront);
                    if (!input.DriverLicenseBackToken.IsNullOrEmpty())
                        insuredPerson.DriverLicenseBack = await _fileOrgService.GetBinaryObjectFromCache(input.DriverLicenseBackToken, input.RegisterId, EnumFolderField.InsuredOwnerDrivingLicenseBack);
                    if (!input.DriverEmploymentDetailToken.IsNullOrEmpty())
                        insuredPerson.DriverEmploymentDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverEmploymentDetailToken, input.RegisterId, EnumFolderField.InsuredOwnerEmploymentDoc);
                    if (!input.DriverHospitalDetailToken.IsNullOrEmpty())
                        insuredPerson.DriverHospitalDetail = await _fileOrgService.GetBinaryObjectFromCache(input.DriverHospitalDetailToken, input.RegisterId, EnumFolderField.InsuredOwnerHospitalDoc);
                    if (!input.DriverCarGrantToken.IsNullOrEmpty())
                        insuredPerson.DriverCarGrant = await _fileOrgService.GetBinaryObjectFromCache(input.DriverCarGrantToken, input.RegisterId, EnumFolderField.InsuredOwnerCarGrant);
                }
            }

            return showInconsistencyError;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _insuredPersonRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_InsuredPersons)]
        public async Task<List<InsuredPersonMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new InsuredPersonMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons)]
        public async Task<List<CommonHospitalDropdownDto>> GetAllHospitalForTableDropdown()
        {
            return await _lookup_hospitalRepository.GetAll()
                .Select(hospital => new CommonHospitalDropdownDto
                {
                    Id = hospital.Id,
                    DisplayName = hospital == null || hospital.Name == null ? "" : hospital.Name.ToString(),
                    Address = hospital.Address
                }).ToListAsync();
        }

        public async Task<List<CommonDropdownDto>> GetAllLocationForTableDropdown(int parentLocationId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

                var location = await _lookup_locationRepository.GetAll().Where(w => w.ParentLocationId == parentLocationId)
                .Select(location => new CommonDropdownDto
                {
                    Id = location.Id,
                    DisplayName = location == null || location.Name == null ? "" : location.Name.ToString()
                }).ToListAsync();

                return location;

            }
        }

        //protected virtual async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        //{
        //    if (fileToken.IsNullOrWhiteSpace())
        //    {
        //        return null;
        //    }

        //    var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

        //    if (fileCache == null)
        //    {
        //        throw new UserFriendlyException("There is no such file with the token: " + fileToken);
        //    }

        //    var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
        //    await _binaryObjectManager.SaveAsync(storedFile);

        //    return storedFile.Id;
        //}

        //protected virtual async Task<string> GetBinaryFileName(Guid? fileId)
        //{
        //    if (!fileId.HasValue)
        //    {
        //        return null;
        //    }

        //    var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
        //    return file?.Description;
        //}

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverICFrontFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverICFront.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverICFront.Value);
            insuredPerson.DriverICFront = null;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverICBackFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverICBack.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverICBack.Value);
            insuredPerson.DriverICBack = null;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverLicenseFrontFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverLicenseFront.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverLicenseFront.Value);
            insuredPerson.DriverLicenseFront = null;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverLicenseBackFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverLicenseBack.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverLicenseBack.Value);
            insuredPerson.DriverLicenseBack = null;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverEmploymentDetailFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverEmploymentDetail.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverEmploymentDetail.Value);
            insuredPerson.DriverEmploymentDetail = null;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverHospitalDetailFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverHospitalDetail.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverHospitalDetail.Value);
            insuredPerson.DriverHospitalDetail = null;
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverCarGrantFile(EntityDto input)
        {
            var insuredPerson = await _insuredPersonRepository.FirstOrDefaultAsync(input.Id);
            if (insuredPerson == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!insuredPerson.DriverCarGrant.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(insuredPerson.DriverCarGrant.Value);
            insuredPerson.DriverCarGrant = null;
        }

        private static bool ValidateLicenseDateRange(DateTime? licenseDateFrom, DateTime? licenseDateTo)
        {
            if (!licenseDateFrom.HasValue && !licenseDateTo.HasValue)
            {
                return true;
            }

            return licenseDateTo >= licenseDateFrom;
        }
    }

}
