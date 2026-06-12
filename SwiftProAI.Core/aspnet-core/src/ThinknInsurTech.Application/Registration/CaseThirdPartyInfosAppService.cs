using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos)]
    public class CaseThirdPartyInfosAppService : ThinknInsurTechAppServiceBase, ICaseThirdPartyInfosAppService
    {
        private readonly IRepository<CaseThirdPartyInfo> _caseThirdPartyInfoRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<Hospital, int> _lookup_hospitalRepository;
        private readonly IRepository<CaseInsuredPerson, int> _lookup_caseInsuredPersonRepository;
        private readonly IFolderService _folderService;
        private readonly IFileOrgService _fileOrgService;
        private readonly IRepository<FileOrg, int> _fileOrgRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;
        private readonly IRepository<CasePoliceReport> _casePoliceReportRepository;
        private readonly IRepository<CasePoliceReportSummary, int> _casePoliceReportSummaryRepository;

        private readonly ITempFileCacheManager _tempFileCacheManager;


        public CaseThirdPartyInfosAppService(IRepository<CaseThirdPartyInfo> caseThirdPartyInfoRepository, IFileOrgService fileOrgService, IRepository<FileOrg, int> fileOrgRepository,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository, IFolderService folderService, IRepository<Hospital, int> lookup_hospitalRepository, IRepository<CaseInsuredPerson, int> lookup_caseInsuredPersonRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<CasePoliceReport> casePoliceReportRepository, IRepository<CasePoliceReportSummary, int> casePoliceReportSummaryRepository)
        {
            _caseThirdPartyInfoRepository = caseThirdPartyInfoRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_hospitalRepository = lookup_hospitalRepository;
            _lookup_caseInsuredPersonRepository = lookup_caseInsuredPersonRepository;
            _folderService = folderService;
            _fileOrgService = fileOrgService;
            _fileOrgRepository = fileOrgRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
            _casePoliceReportRepository = casePoliceReportRepository;
            _casePoliceReportSummaryRepository = casePoliceReportSummaryRepository;

        }

        public virtual async Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAll(GetAllCaseThirdPartyInfosInput input)
        {

            var filteredCaseThirdPartyInfos = _caseThirdPartyInfoRepository.GetAll()
                    .Include(e => e.RegisterFk)
                    .Include(e => e.HospitalId1Fk)
                    .Include(e => e.CaseInsuredPersonFk)
                    .Where(e => e.RegisterId.Equals(input.RegisterId));

            var pagedAndFilteredCaseThirdPartyInfos = filteredCaseThirdPartyInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseThirdPartyInfos = from o in pagedAndFilteredCaseThirdPartyInfos
                                      join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_hospitalRepository.GetAll() on o.HospitalId1 equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      join o5 in _lookup_caseInsuredPersonRepository.GetAll() on o.CaseInsuredPersonId equals o5.Id into j5
                                      from s5 in j5.DefaultIfEmpty()

                                      select new
                                      {

                                          o.Age,
                                          o.Sex,
                                          o.MaritalStatus,
                                          o.ThirdPartyType,
                                          o.AdmittedDate1,
                                          o.AdmittedDate2,
                                          o.AdmittedDate3,
                                          o.DischargeDate1,
                                          o.DischargeDate2,
                                          o.DischargeDate3,
                                          o.EmployerPrior,
                                          o.EmployedDateFrom,
                                          o.EmployedDateTo,
                                          o.EPF,
                                          o.SOCSO,
                                          o.MedicalBenefit,
                                          o.IncomeLoss,
                                          o.EmployerAdministrative,
                                          o.AfterAccidentEmployerName,
                                          o.AfterAccidentEmployerIncome,
                                          o.AfterAccidentEmployerIncomeReduction,
                                          o.AfterAccidentEmployerAddress,
                                          o.AfterAccidentEmployerJob,
                                          o.InjuriesSustained,
                                          o.MedicalLeave,
                                          o.DisablementPeriodFrom,
                                          o.DisablementPeriodTo,
                                          o.PresentCondition,
                                          o.CurrentDisabilities,
                                          o.SolicitorName,
                                          o.SolicitorAddress,
                                          o.SolicitorContact,
                                          o.SolicitorReferenceNo,
                                          o.OtherMedicalBenefit,
                                          o.FatalCaseCheck,
                                          o.VehicleNo,
                                          Id = o.Id,
                                          MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                          HospitalName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                          CaseInsuredPersonName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                                      };

            var totalCount = await filteredCaseThirdPartyInfos.CountAsync();

            var dbList = await caseThirdPartyInfos.ToListAsync();
            var results = new List<GetCaseThirdPartyInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseThirdPartyInfoForViewDto()
                {
                    CaseThirdPartyInfo = new CaseThirdPartyInfoDto
                    {

                        Age = o.Age,
                        Sex = o.Sex,
                        MaritalStatus = o.MaritalStatus,
                        ThirdPartyType = o.ThirdPartyType,
                        AdmittedDate1 = o.AdmittedDate1,
                        AdmittedDate2 = o.AdmittedDate2,
                        AdmittedDate3 = o.AdmittedDate3,
                        DischargeDate1 = o.DischargeDate1,
                        DischargeDate2 = o.DischargeDate2,
                        DischargeDate3 = o.DischargeDate3,
                        EmployerPrior = o.EmployerPrior,
                        EmployedDateFrom = o.EmployedDateFrom,
                        EmployedDateTo = o.EmployedDateTo,
                        EPF = o.EPF,
                        SOCSO = o.SOCSO,
                        MedicalBenefit = o.MedicalBenefit,
                        IncomeLoss = o.IncomeLoss,
                        EmployerAdministrative = o.EmployerAdministrative,
                        AfterAccidentEmployerName = o.AfterAccidentEmployerName,
                        AfterAccidentEmployerIncome = o.AfterAccidentEmployerIncome,
                        AfterAccidentEmployerIncomeReduction = o.AfterAccidentEmployerIncomeReduction,
                        AfterAccidentEmployerAddress = o.AfterAccidentEmployerAddress,
                        AfterAccidentEmployerJob = o.AfterAccidentEmployerJob,
                        InjuriesSustained = o.InjuriesSustained,
                        MedicalLeave = o.MedicalLeave,
                        DisablementPeriodFrom = o.DisablementPeriodFrom,
                        DisablementPeriodTo = o.DisablementPeriodTo,
                        PresentCondition = o.PresentCondition,
                        CurrentDisabilities = o.CurrentDisabilities,
                        SolicitorName = o.SolicitorName,
                        SolicitorAddress = o.SolicitorAddress,
                        SolicitorContact = o.SolicitorContact,
                        SolicitorReferenceNo = o.SolicitorReferenceNo,
                        OtherMedicalBenefit = o.OtherMedicalBenefit,
                        FatalCaseCheck = o.FatalCaseCheck,
                        VehicleNo = o.VehicleNo,
                        Id = o.Id,
                    },
                    MainRegistrationVehicleNo = o.MainRegistrationVehicleNo,
                    HospitalName = o.HospitalName,
                    CaseInsuredPersonName = o.CaseInsuredPersonName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseThirdPartyInfoForViewDto>(
                totalCount,
                results
            );


        }

        public async Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAllForView(GetAllCaseThirdPartyInfosInput input)
        {

            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;
                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == input.RegisterId).Select(f => f.RegisterId).FirstOrDefault();

                var filteredCaseThirdPartyInfos = _caseThirdPartyInfoRepository.GetAll()
                        .Include(e => e.RegisterFk)
                        .Include(e => e.HospitalId1Fk)
                        .Include(e => e.CaseInsuredPersonFk)
                        .Where(e => e.RegisterId == assignedRegisterId);

                var pagedAndFilteredCaseThirdPartyInfos = filteredCaseThirdPartyInfos
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var caseThirdPartyInfos = from o in pagedAndFilteredCaseThirdPartyInfos
                                          join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                          from s1 in j1.DefaultIfEmpty()

                                          join o2 in _lookup_hospitalRepository.GetAll() on o.HospitalId1 equals o2.Id into j2
                                          from s2 in j2.DefaultIfEmpty()

                                          join o5 in _lookup_caseInsuredPersonRepository.GetAll() on o.CaseInsuredPersonId equals o5.Id into j5
                                          from s5 in j5.DefaultIfEmpty()

                                          select new
                                          {

                                              o.Age,
                                              o.Sex,
                                              o.MaritalStatus,
                                              o.ThirdPartyType,
                                              o.AdmittedDate1,
                                              o.AdmittedDate2,
                                              o.AdmittedDate3,
                                              o.DischargeDate1,
                                              o.DischargeDate2,
                                              o.DischargeDate3,
                                              o.EmployerPrior,
                                              o.EmployedDateFrom,
                                              o.EmployedDateTo,
                                              o.EPF,
                                              o.SOCSO,
                                              o.MedicalBenefit,
                                              o.IncomeLoss,
                                              o.EmployerAdministrative,
                                              o.AfterAccidentEmployerName,
                                              o.AfterAccidentEmployerIncome,
                                              o.AfterAccidentEmployerIncomeReduction,
                                              o.AfterAccidentEmployerAddress,
                                              o.AfterAccidentEmployerJob,
                                              o.InjuriesSustained,
                                              o.MedicalLeave,
                                              o.DisablementPeriodFrom,
                                              o.DisablementPeriodTo,
                                              o.PresentCondition,
                                              o.CurrentDisabilities,
                                              o.SolicitorName,
                                              o.SolicitorAddress,
                                              o.SolicitorContact,
                                              o.SolicitorReferenceNo,
                                              o.OtherMedicalBenefit,
                                              o.FatalCaseCheck,
                                              o.VehicleNo,
                                              Id = o.Id,
                                              MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                              HospitalName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                              CaseInsuredPersonName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                                          };

                var totalCount = await filteredCaseThirdPartyInfos.CountAsync();

                var dbList = await caseThirdPartyInfos.ToListAsync();
                var results = new List<GetCaseThirdPartyInfoForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCaseThirdPartyInfoForViewDto()
                    {
                        CaseThirdPartyInfo = new CaseThirdPartyInfoDto
                        {

                            Age = o.Age,
                            Sex = o.Sex,
                            MaritalStatus = o.MaritalStatus,
                            ThirdPartyType = o.ThirdPartyType,
                            AdmittedDate1 = o.AdmittedDate1,
                            AdmittedDate2 = o.AdmittedDate2,
                            AdmittedDate3 = o.AdmittedDate3,
                            DischargeDate1 = o.DischargeDate1,
                            DischargeDate2 = o.DischargeDate2,
                            DischargeDate3 = o.DischargeDate3,
                            EmployerPrior = o.EmployerPrior,
                            EmployedDateFrom = o.EmployedDateFrom,
                            EmployedDateTo = o.EmployedDateTo,
                            EPF = o.EPF,
                            SOCSO = o.SOCSO,
                            MedicalBenefit = o.MedicalBenefit,
                            IncomeLoss = o.IncomeLoss,
                            EmployerAdministrative = o.EmployerAdministrative,
                            AfterAccidentEmployerName = o.AfterAccidentEmployerName,
                            AfterAccidentEmployerIncome = o.AfterAccidentEmployerIncome,
                            AfterAccidentEmployerIncomeReduction = o.AfterAccidentEmployerIncomeReduction,
                            AfterAccidentEmployerAddress = o.AfterAccidentEmployerAddress,
                            AfterAccidentEmployerJob = o.AfterAccidentEmployerJob,
                            InjuriesSustained = o.InjuriesSustained,
                            MedicalLeave = o.MedicalLeave,
                            DisablementPeriodFrom = o.DisablementPeriodFrom,
                            DisablementPeriodTo = o.DisablementPeriodTo,
                            PresentCondition = o.PresentCondition,
                            CurrentDisabilities = o.CurrentDisabilities,
                            SolicitorName = o.SolicitorName,
                            SolicitorAddress = o.SolicitorAddress,
                            SolicitorContact = o.SolicitorContact,
                            SolicitorReferenceNo = o.SolicitorReferenceNo,
                            OtherMedicalBenefit = o.OtherMedicalBenefit,
                            FatalCaseCheck = o.FatalCaseCheck,
                            VehicleNo = o.VehicleNo,
                            Id = o.Id,
                        },
                        MainRegistrationVehicleNo = o.MainRegistrationVehicleNo,
                        HospitalName = o.HospitalName,
                        CaseInsuredPersonName = o.CaseInsuredPersonName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetCaseThirdPartyInfoForViewDto>(
                    totalCount,
                    results
                );
            }

        }


        protected async Task checkValidationForThirdPartyInfo(string userIc, int registerId, int? id)
        {

            var isRegistered = await _caseThirdPartyInfoRepository.GetAll()
                .WhereIf(id != null && id > 0, f => f.Id != id)
                .Where(f => f.RegisterId == registerId)
                .Join(_lookup_caseInsuredPersonRepository.GetAll(),
                      thirdPartyInfo => thirdPartyInfo.CaseInsuredPersonId,
                      insuredPerson => insuredPerson.Id,
                      (thirdPartyInfo, insuredPerson) => insuredPerson.IdenticationNo)
                .AnyAsync(ic => ic == userIc);

            if (isRegistered)
            {
                throw new UserFriendlyException("User already registered under this case");
            }


        }
        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos_Edit)]
        public virtual async Task<GetCaseThirdPartyInfoForEditOutput> GetCaseThirdPartyInfoForEdit(EntityDto input)
        {

            var caseThirdPartyInfo = await _caseThirdPartyInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCaseThirdPartyInfoForEditOutput { CaseThirdPartyInfo = ObjectMapper.Map<CreateOrEditCaseThirdPartyInfoDto>(caseThirdPartyInfo) };

            //if (output.CaseThirdPartyInfo.RegisterId != null)
            //{
            //    var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.RegisterId);
            //    output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            //}

            if (output.CaseThirdPartyInfo.HospitalId1 != null)
            {
                var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.HospitalId1);
                output.HospitalName = _lookupHospital?.Name?.ToString();
                output.HospitalAddress = _lookupHospital?.Address?.ToString();
            }

            if (output.CaseThirdPartyInfo.HospitalId2 != null)
            {
                var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.HospitalId2);
                output.HospitalName2 = _lookupHospital?.Name?.ToString();
                output.HospitalAddress2 = _lookupHospital?.Address?.ToString();
            }

            if (output.CaseThirdPartyInfo.HospitalId3 != null)
            {
                var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.HospitalId3);
                output.HospitalName3 = _lookupHospital?.Name?.ToString();
                output.HospitalAddress3 = _lookupHospital?.Address?.ToString();
            }

            if (output.CaseThirdPartyInfo.CaseInsuredPersonId != null)
            {
                var _lookupCaseInsuredPerson = await _lookup_caseInsuredPersonRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.CaseInsuredPersonId);
                output.CaseInsuredPerson = ObjectMapper.Map<CreateOrEditInsuredPersonDto>(_lookupCaseInsuredPerson);
                output.CaseThirdPartyInfo.CaseInsuredPerson = ObjectMapper.Map<CreateOrEditInsuredPersonDto>(_lookupCaseInsuredPerson);
            }

            await GetTPIFile(output.CaseThirdPartyInfo, input.Id);
            return output;
        }

        public virtual async Task<GetCaseThirdPartyInfoForViewDto> GetThirdPartyInfoForView(int id)
        {

            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var caseThirdPartyInfo = await _caseThirdPartyInfoRepository.FirstOrDefaultAsync(id);

                var output = new GetCaseThirdPartyInfoForViewDto { CaseThirdPartyInfo = ObjectMapper.Map<CaseThirdPartyInfoDto>(caseThirdPartyInfo) };

                var currentOUId = AbpSession.GetCurrentOUId().Value;

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == caseThirdPartyInfo.RegisterId).Select(f => f.RegisterId).FirstOrDefault();

                if (assignedRegisterId == 0 || assignedRegisterId == null)
                {
                    return new GetCaseThirdPartyInfoForViewDto();
                }

                //if (output.CaseThirdPartyInfo.RegisterId != null)
                //{
                //    var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.RegisterId);
                //    output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
                //}


                if (output.CaseThirdPartyInfo.HospitalId1 != null)
                {
                    var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.HospitalId1);
                    output.HospitalName = _lookupHospital?.Name?.ToString();
                    output.HospitalAddress = _lookupHospital?.Address?.ToString();
                }

                if (output.CaseThirdPartyInfo.HospitalId2 != null)
                {
                    var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.HospitalId2);
                    output.HospitalName2 = _lookupHospital?.Name?.ToString();
                    output.HospitalAddress2 = _lookupHospital?.Address?.ToString();
                }

                if (output.CaseThirdPartyInfo.HospitalId3 != null)
                {
                    var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.HospitalId3);
                    output.HospitalName3 = _lookupHospital?.Name?.ToString();
                    output.HospitalAddress3 = _lookupHospital?.Address?.ToString();
                }

                if (output.CaseThirdPartyInfo.CaseInsuredPersonId != null)
                {
                    var _lookupCaseInsuredPerson = await _lookup_caseInsuredPersonRepository.FirstOrDefaultAsync((int)output.CaseThirdPartyInfo.CaseInsuredPersonId);
                    output.InsuredPerson = ObjectMapper.Map<InsuredPersonDto>(_lookupCaseInsuredPerson);
                    output.CaseThirdPartyInfo.CaseInsuredPerson = ObjectMapper.Map<InsuredPersonDto>(_lookupCaseInsuredPerson);
                }
                await GetTPIFileForView(output, output.CaseThirdPartyInfo.Id);
                return output;
            }
        }

        public virtual async Task<bool?> CreateOrEdit(CreateOrEditCaseThirdPartyInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos_Create)]
        protected virtual async Task Create(CreateOrEditCaseThirdPartyInfoDto input)
        {
            await checkValidationForThirdPartyInfo(input.CaseInsuredPerson.IdenticationNo, (int)input.RegisterId, null);
            if (!ValidateDateRange(input.EmployedDateFrom, input.EmployedDateTo))
            {
                throw new UserFriendlyException("Employee Date To must be greater than Employee Date From");
            }

            if (!ValidateDateRange(input.CaseInsuredPerson.LicenseDateFrom, input.CaseInsuredPerson.LicenseDateTo))
            {
                throw new UserFriendlyException("License Date To must be greater than License Date From");
            }

            var caseThirdPartyInfo = ObjectMapper.Map<CaseThirdPartyInfo>(input);

            if (AbpSession.TenantId != null)
            {
                caseThirdPartyInfo.TenantId = (int)AbpSession.TenantId;
            }
            caseThirdPartyInfo.CaseInsuredPersonFk = ObjectMapper.Map<CaseInsuredPerson>(input.CaseInsuredPerson);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseThirdPartyInfo.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;
                caseThirdPartyInfo.CaseInsuredPersonFk.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;
            }

            await _caseThirdPartyInfoRepository.InsertAsync(caseThirdPartyInfo);
            await CurrentUnitOfWork.SaveChangesAsync();

            await CreateThirdPartyInfoFolderList((int)caseThirdPartyInfo.Id, input.CaseInsuredPerson.IdenticationNo);

            await SaveTPIFile(input, caseThirdPartyInfo.Id);

            await CasePoliceReportICValidation(input.CaseInsuredPerson.RegisterId, input.CaseInsuredPerson.IdenticationNo);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseThirdPartyInfoRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos_Delete)]
        public async Task RemoveTpiFile(RemoveFile input)
        {


            //var thirdPartyInfo = _caseThirdPartyInfoRepository.FirstOrDefault(x => x.Id == input.ThirdPartyInfoId);

            //var insuredsPersons = thirdPartyInfo.CaseInsuredPersonId == null ?
            //    null :
            //    await _lookup_caseInsuredPersonRepository.FirstOrDefaultAsync(x => x.Id == input.ThirdPartyInfoId);



            await _fileOrgService.DeleteFileByReference(input.FileToken);

            //switch (docTypeEnum)
            //{
            //    case UploadDocTypeEnum.None:
            //    case UploadDocTypeEnum.PoliceReport:
            //        break;
            //    case UploadDocTypeEnum.LicenseFront:
            //        insuredsPersons.DriverLicenseFront = null;
            //        break;
            //    case UploadDocTypeEnum.LicenseBack:
            //        insuredsPersons.DriverLicenseBack = null;
            //        break;
            //    case UploadDocTypeEnum.ICFront:
            //        insuredsPersons.DriverICFront = null;
            //        break;
            //    // IC BACK Does Not Have OCR
            //    //case UploadDocTypeEnum.ICBack:
            //    //    insuredsPersons.DriverEmploymentDetail = null;
            //    //    break;
            //    case UploadDocTypeEnum.EmploymentDetail:
            //        insuredsPersons.DriverEmploymentDetail = null;
            //        break;

            //}
        }

        protected async Task ChangeDirectory(int registerId, int id, string newIdentificationNo, string oldIdentifcationNo)
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                //Process delete
                var folders = await _folderService.GetAllByMainEntityAndIdAsync($"ThirdPartyInfo - {oldIdentifcationNo}", id);
                await _fileOrgService.DeleteFileByMainEntityAndMainEntityID("ThirdPartyInfo", id);

                foreach (var folder in folders)
                {
                    await _folderService.DeleteFolder(folder.Id, registerId);
                }

                //Process Create
                await CreateThirdPartyInfoFolderList(id, newIdentificationNo);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos)]
        public async Task<List<CaseThirdPartyInfoMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CaseThirdPartyInfoMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }


        #region Helper Methods
        //TODO
        private async Task GetTPIFile(CreateOrEditCaseThirdPartyInfoDto input, int thirdPartyId)
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var folders = GetAllThirdPartyFolder((int)input.Id, input.CaseInsuredPerson.IdenticationNo) ?? null;

                if (folders == null)
                {
                    return;
                }

                var folderIds = folders.Select(f => f.Id).ToList();

                var files = _fileOrgRepository.GetAll()
                    .Where(f => folderIds.Contains((int)f.FolderId))
                    .ToList();



                var folderFiles = folders.Join(
                    files,
                    folder => folder.Id,
                    file => file.FolderId,
                    (folder, file) => new
                    {
                        Folder = folder,
                        File = file
                    }).ToList();

                //file token is not equal to file reference number
                foreach (var folderFile in folderFiles)
                {
                    switch (folderFile.Folder.Field)
                    {
                        case "THP-EMP":
                            input.EmpFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.EmpReferenceNo = folderFile.File.ReferenceNo.ToString();
                            break;

                        case "THP-DET":
                            input.DetFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.DetReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-DL-DET-B":
                            input.DlDetBackFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.DlDetBackReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-DL-DET-F":
                            input.DlDetFrontFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.DlDetFrontReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-DL-NRIC-B":
                            input.DlNricBackFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.DlNricBackReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-NRIC-DET-F":
                            input.NricDetFrontFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.NricDetFrontReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-NOI-DET":
                            input.NoiFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.NoiReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-HOSPITAL":
                            input.HospFileName = await _fileOrgService.GetBinaryFileName(folderFile.File.ReferenceNo);
                            input.HospReferenceNo = folderFile.File.ReferenceNo.ToString();

                            break;
                    }
                }
            }

        }

        //TODO
        private async Task GetTPIFileForView(GetCaseThirdPartyInfoForViewDto input, int thirdPartyId)
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var folders = GetAllThirdPartyFolder((int)input.CaseThirdPartyInfo.Id, input.InsuredPerson.IdenticationNo) ?? null;

                if (folders == null)
                {
                    return;
                }

                var folderIds = folders.Select(f => f.Id).ToList();

                var files = _fileOrgRepository.GetAll()
                    .Where(f => folderIds.Contains((int)f.FolderId))
                    .ToList();



                // Perform the join between folders and files
                var folderFiles = folders.Join(
                    files,
                    folder => folder.Id,
                    file => file.FolderId,
                    (folder, file) => new
                    {
                        Folder = folder,
                        File = file
                    }).ToList();

                // Iterate over the joined result to assign the file names
                foreach (var folderFile in folderFiles)
                {
                    switch (folderFile.Folder.Field)
                    {
                        case "THP-EMP":
                            input.EmpFileName = folderFile.File.FileName;
                            input.EmpToken = folderFile.File.ReferenceNo.ToString();
                            break;
                        case "THP-DET":
                            input.DetFileName = folderFile.File.FileName;
                            input.DetToken = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-DL-DET-B":
                            input.DlDetBackFileName = folderFile.File.FileName;
                            input.DlDetBackToken = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-DL-DET-F":
                            input.DlDetFrontFileName = folderFile.File.FileName;

                            input.DlDetFrontToken = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-DL-NRIC-B":
                            input.DlNricBackFileName = folderFile.File.FileName;
                            input.DlNricBackToken = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-NRIC-DET-F":
                            input.NricDetFrontFileName = folderFile.File.FileName;
                            input.NricDetFrontToken = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-NOI-DET":
                            input.NoiFileName = folderFile.File.FileName;

                            input.NoiToken = folderFile.File.ReferenceNo.ToString();

                            break;
                        case "THP-HOSPITAL":
                            input.HospFileName = folderFile.File.FileName;
                            input.HospToken = folderFile.File.ReferenceNo.ToString();

                            break;
                    }
                }
            }
            //Get Folder Records



        }

        private async Task SaveTPIFile(CreateOrEditCaseThirdPartyInfoDto input, int thirdPartyId)
        {
            //Create Folder records
            var folders = GetAllThirdPartyFolder(thirdPartyId, input.CaseInsuredPerson.IdenticationNo);
            var registerId = input.RegisterId;

            if (input.EmpToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-EMP");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.EmpToken, (int)registerId, folder.Id);
            }
            if (input.DetToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-DET");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.DetToken, (int)registerId, folder.Id);
            }
            if (input.DlDetBackToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-DL-DET-B");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.DlDetBackToken, (int)registerId, folder.Id);
            }
            if (input.DlDetFrontToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-DL-DET-F");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.DlDetFrontToken, (int)registerId, folder.Id);
            }
            if (input.DlNricBackToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-DL-NRIC-B");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.DlNricBackToken, (int)registerId, folder.Id);
            }
            if (input.NricDetFrontToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-NRIC-DET-F");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.NricDetFrontToken, (int)registerId, folder.Id);
            }
            if (input.NoiToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-NOI-DET");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.NoiToken, (int)registerId, folder.Id);
            }
            if (input.HospToken != null)
            {
                var folder = folders.FirstOrDefault(f => f.Field == "THP-HOSPITAL");
                await _fileOrgService.GetBinaryObjectFromCacheToId(input.HospToken, (int)registerId, folder.Id);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyInfos_Edit)]
        protected virtual async Task<bool?> Update(CreateOrEditCaseThirdPartyInfoDto input)
        {
            await checkValidationForThirdPartyInfo(input.CaseInsuredPerson.IdenticationNo, (int)input.RegisterId, input.Id);
            if (!ValidateDateRange(input.EmployedDateFrom, input.EmployedDateTo))
            {
                throw new UserFriendlyException("Employee Date To must be greater than Employee Date From");
            }

            if (!ValidateDateRange(input.CaseInsuredPerson.LicenseDateFrom, input.CaseInsuredPerson.LicenseDateTo))
            {
                throw new UserFriendlyException("License Date To must be greater than License Date From");
            }

            var ctpiList = await _caseThirdPartyInfoRepository.GetAll()
                .Where(x => x.Id == input.Id)
                .Join(_lookup_caseInsuredPersonRepository.GetAll(), ctpi => ctpi.CaseInsuredPersonId, cip => cip.Id, (ctpi, cip) => new { ctpi, cip })
                .FirstOrDefaultAsync();
            if (ctpiList.cip.IdenticationNo != input.CaseInsuredPerson.IdenticationNo)
            {
                await ChangeDirectory((int)input.RegisterId, (int)input.Id, input.CaseInsuredPerson.IdenticationNo, ctpiList.cip.IdenticationNo);
            }

            var result = await CasePoliceReportICValidation(input.CaseInsuredPerson.RegisterId, input.CaseInsuredPerson.IdenticationNo, ctpiList.cip.IdenticationNo);

            ObjectMapper.Map(input, ctpiList.ctpi);
            ObjectMapper.Map(input.CaseInsuredPerson, ctpiList.cip);


            GetAllFoldersInput getAllFoldersInput = new GetAllFoldersInput
            {
                FieldFilter = "",
                MainEntityFilter = FolderConsts.ThirdPartyInfoMainEntity,
                MainEntityIdFilter = (int)input.Id,
            };



            if (!_folderService.GetAll(getAllFoldersInput).Any())
            {
                await CreateThirdPartyInfoFolderList((int)input.Id, input.CaseInsuredPerson.IdenticationNo);
            }
            await SaveTPIFile(input, (int)input.Id);

            return result;
        }


        private List<FolderDto> GetAllThirdPartyFolder(int thirdPartyId, string idNumber)
        {
            GetAllFoldersInput getAllFoldersInput = new GetAllFoldersInput
            {
                FieldFilter = "",
                MainEntityFilter = $"{FolderConsts.ThirdPartyInfoMainEntity} - {idNumber}",
                MainEntityIdFilter = thirdPartyId,
            };

            var folders = _folderService.GetAll(getAllFoldersInput);

            return folders;
        }

        private async Task<List<FolderDto>> CreateThirdPartyInfoFolderList(
            int thirdPartyId,
            string idNumber
        )
        {
            var folderDtos = new List<FolderDto>();

            foreach (var field in FolderConsts.ThirdPartyFields)
            {


                var folderId = await _folderService.CreateByMainEntityAndField($"{FolderConsts.ThirdPartyInfoMainEntity} - {idNumber}", field, thirdPartyId);

                var folderDto = new FolderDto
                {
                    Id = folderId,
                    Field = field,
                    MainEntity = $"{FolderConsts.ThirdPartyInfoMainEntity} - {idNumber}",
                    MainEntityId = thirdPartyId
                };

                folderDtos.Add(folderDto);
            }

            return folderDtos;
        }
        #endregion

        private static bool ValidateDateRange(DateTime? dateFrom, DateTime? dateTo)
        {
            if (!dateFrom.HasValue && !dateTo.HasValue)
            {
                return true;
            }

            return dateTo >= dateFrom;
        }

        private async Task<bool> CasePoliceReportICValidation(int registerId, string identityNo, string prevIdentityNo = null)
        {
            var showInconsistencyError = false;
            var inputIdentityNo = Regex.Replace(identityNo, @"[\s-]", "");
            var prevInsPersonIdentityNo = prevIdentityNo != null ? Regex.Replace(prevIdentityNo, @"[\s-]", "") : null;

            var cprList = await _casePoliceReportRepository.GetAll()
                .Where(x => x.ReportType == "ThirdParty" && x.RegisterId == registerId)
                .ToListAsync();

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
                            .Where(x => x.RegisterId.Equals(registerId))
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

            return showInconsistencyError;
        }


    }
}