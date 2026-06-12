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
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CasePoliceReports)]
    public class CasePoliceReportsAppService : ThinknInsurTechAppServiceBase, ICasePoliceReportsAppService
    {
        private readonly IRepository<CasePoliceReport> _casePoliceReportRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<CaseIncidentDetail, int> _lookup_caseIncidentDetailRepository;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;
        private readonly IRepository<Lookup, int> _lookup_lookupRepository;
        private readonly IRepository<CaseInsuredPerson, int> _caseInsuredPersonRepository;
        private readonly IRepository<CaseThirdPartyInfo, int> _caseThirdPartyInfoRepository;

        private readonly IFolderService _folderService;
        private readonly IFileOrgService _fileOrgService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CasePoliceReportsAppService(
            IRepository<CasePoliceReport> casePoliceReportRepository,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository,
            IRepository<CaseIncidentDetail, int> lookup_caseIncidentDetailRepository,
            IRepository<Lookup, int> lookup_lookupRepository,
            IRepository<CaseInsuredPerson, int> caseInsuredPersonRepository,
            IRepository<CaseThirdPartyInfo, int> caseThirdPartyInfoRepository,

            IFolderService folderService,
            IFileOrgService fileOrgService,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository
            )
        {
            _casePoliceReportRepository = casePoliceReportRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_caseIncidentDetailRepository = lookup_caseIncidentDetailRepository;
            _lookup_lookupRepository = lookup_lookupRepository;
            _caseInsuredPersonRepository = caseInsuredPersonRepository;
            _caseThirdPartyInfoRepository = caseThirdPartyInfoRepository;

            _folderService = folderService;
            _fileOrgService = fileOrgService;
            _unitOfWorkManager = unitOfWorkManager;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;

        }

        public virtual async Task<PagedResultDto<GetCasePoliceReportForViewDto>> GetAll(GetAllCasePoliceReportsInput input)
        {
            var filteredCasePoliceReports = _casePoliceReportRepository.GetAll()
                         .Include(e => e.RegisterFk)
                         .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id.ToString() == input.RegisterIdFilter);

            var pagedAndFilteredCasePoliceReports = filteredCasePoliceReports
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var casePoliceReports = from o in pagedAndFilteredCasePoliceReports
                                    join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    select new
                                    {
                                        o.RegisterId,
                                        o.IPD,
                                        o.PoliceStation,
                                        o.VehicleNo,
                                        o.ReportNo,
                                        o.ReportTime,
                                        o.IncidentTime,
                                        o.LateReport,
                                        o.LateReason,
                                        o.OfficerName,
                                        o.ServiceNo,
                                        o.OfficerContact,
                                        o.Type,
                                        o.PoliceFinding,
                                        o.PoliceOutcome,
                                        o.ReportFileUpload,
                                        o.Id,
                                        MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                        o.IsDataConsistent,
                                        o.ReportType,
                                        o.ComplainantIdentityNo,
                                        o.Statement
                                    };

            var totalCount = await filteredCasePoliceReports.CountAsync();

            var dbList = await casePoliceReports.ToListAsync();
            var results = new List<GetCasePoliceReportForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCasePoliceReportForViewDto()
                {
                    CasePoliceReport = new CasePoliceReportDto
                    {
                        RegisterId = o.RegisterId,
                        IPD = o.IPD,
                        PoliceStation = o.PoliceStation,
                        VehicleNo = o.VehicleNo,
                        ReportNo = o.ReportNo,
                        ReportTime = o.ReportTime,
                        IncidentTime = o.IncidentTime,
                        LateReport = o.LateReport,
                        LateReason = o.LateReason,
                        OfficerName = o.OfficerName,
                        ServiceNo = o.ServiceNo,
                        OfficerContact = o.OfficerContact,
                        Type = o.Type,
                        PoliceFinding = o.PoliceFinding,
                        PoliceOutcome = o.PoliceOutcome,
                        ReportFileUpload = o.ReportFileUpload,
                        Id = o.Id,
                        IsDataConsistent = o.IsDataConsistent,
                        ReportType = o.ReportType,
                        ComplainantIdentityNo = o.ComplainantIdentityNo,
                        Statement = o.Statement
                    },
                    MainRegistrationVehicleNo = o.MainRegistrationVehicleNo
                };
                res.CasePoliceReport.ReportFileUploadFileName = await _fileOrgService.GetBinaryFileName(o.ReportFileUpload);

                results.Add(res);
            }

            return new PagedResultDto<GetCasePoliceReportForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<PagedResultDto<GetCasePoliceReportForViewDto>> GetAllForView(GetAllCasePoliceReportsInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;
                int registerId = Convert.ToInt32(input.RegisterIdFilter);
                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == registerId).Select(f => f.RegisterId).FirstOrDefault();

                var filteredCasePoliceReports = _casePoliceReportRepository.GetAll()
                        .Include(e => e.RegisterFk)
                            .Where(f => f.RegisterId == assignedRegisterId);

                var pagedAndFilteredCasePoliceReports = filteredCasePoliceReports
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var casePoliceReports = from o in pagedAndFilteredCasePoliceReports
                                        join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        select new
                                        {
                                            o.RegisterId,
                                            o.IPD,
                                            o.PoliceStation,
                                            o.VehicleNo,
                                            o.ReportNo,
                                            o.ReportTime,
                                            o.IncidentTime,
                                            o.LateReport,
                                            o.LateReason,
                                            o.OfficerName,
                                            o.ServiceNo,
                                            o.OfficerContact,
                                            o.Type,
                                            o.PoliceFinding,
                                            o.PoliceOutcome,
                                            o.ReportFileUpload,
                                            o.Id,
                                            MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                            o.IsDataConsistent,
                                            o.ReportType,
                                            o.ComplainantIdentityNo,
                                            o.Statement
                                        };

                var totalCount = await filteredCasePoliceReports.CountAsync();

                var dbList = await casePoliceReports.ToListAsync();
                var results = new List<GetCasePoliceReportForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCasePoliceReportForViewDto()
                    {
                        CasePoliceReport = new CasePoliceReportDto
                        {
                            RegisterId = o.RegisterId,
                            IPD = o.IPD,
                            PoliceStation = o.PoliceStation,
                            VehicleNo = o.VehicleNo,
                            ReportNo = o.ReportNo,
                            ReportTime = o.ReportTime,
                            IncidentTime = o.IncidentTime,
                            LateReport = o.LateReport,
                            LateReason = o.LateReason,
                            OfficerName = o.OfficerName,
                            ServiceNo = o.ServiceNo,
                            OfficerContact = o.OfficerContact,
                            Type = o.Type,
                            PoliceFinding = o.PoliceFinding,
                            PoliceOutcome = o.PoliceOutcome,
                            ReportFileUpload = o.ReportFileUpload,
                            Id = o.Id,
                            IsDataConsistent = o.IsDataConsistent,
                            ReportType = o.ReportType,
                            ComplainantIdentityNo = o.ComplainantIdentityNo,
                            Statement = o.Statement,
                        },
                        MainRegistrationVehicleNo = o.MainRegistrationVehicleNo
                    };
                    res.CasePoliceReport.ReportFileUploadFileName = await _fileOrgService.GetBinaryFileName(o.ReportFileUpload);

                    results.Add(res);
                }

                return new PagedResultDto<GetCasePoliceReportForViewDto>(
                    totalCount,
                    results
                );
            }

        }

        public virtual async Task<GetCasePoliceReportForViewDto> GetCasePoliceReportForView(int id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var casePoliceReport = await _casePoliceReportRepository.GetAsync(id);
                var currentOUId = AbpSession.GetCurrentOUId().Value;

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == casePoliceReport.RegisterId).Select(f => f.RegisterId).FirstOrDefault();

                if (assignedRegisterId == 0 || assignedRegisterId == null)
                {
                    return new GetCasePoliceReportForViewDto();
                }

                var output = new GetCasePoliceReportForViewDto { CasePoliceReport = ObjectMapper.Map<CasePoliceReportDto>(casePoliceReport) };

                if (output.CasePoliceReport.RegisterId != null)
                {
                    var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CasePoliceReport.RegisterId);
                    output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
                }

                output.CasePoliceReport.ReportFileUploadFileName = await _fileOrgService.GetBinaryFileName(casePoliceReport.ReportFileUpload);

                return output;

            }

        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReports_Edit)]
        public virtual async Task<GetCasePoliceReportForEditOutput> GetCasePoliceReportForEdit(GetCasePoliceReportForEditInput input)
        {
            var casePoliceReport = _casePoliceReportRepository.GetAll().Where(w => w.Id.Equals(input.Id)).FirstOrDefault();
            var output = new GetCasePoliceReportForEditOutput { CasePoliceReport = ObjectMapper.Map<CreateOrEditCasePoliceReportDto>(casePoliceReport) };

            //var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);
            //output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            if (casePoliceReport != null)
            {
                output.ReportFileUploadFileName = await _fileOrgService.GetBinaryFileName(casePoliceReport.ReportFileUpload);

                if (output.ReportFileUploadFileName == null) // Clean up the ReportFileUpload GUID if file is deleted
                {
                    casePoliceReport.ReportFileUpload = null;
                }
            }

            var caseIncidentDetail = _lookup_caseIncidentDetailRepository.GetAll().Where(w => w.RegisterId.Equals(input.RegisterId)).FirstOrDefault();
            if (caseIncidentDetail != null)
            {
                output.IncidentTimeTo = caseIncidentDetail.TimeTo;
            }

            //TODO: add the registerId as another input, currently the input is the casepolicereport's id. then use registerId to get incidenttime
            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCasePoliceReportDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReports_Create)]
        protected virtual async Task Create(CreateOrEditCasePoliceReportDto input)
        {
            var existingCasePoliceReport = await _casePoliceReportRepository.FirstOrDefaultAsync(x => x.RegisterId == input.RegisterId && x.ReportType == input.ReportType);

            if(existingCasePoliceReport != null && input.ReportType != "ThirdParty")
            {
                throw new UserFriendlyException(input.ReportType + " report has been uploaded");
            }

            var casePoliceReport = ObjectMapper.Map<CasePoliceReport>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                casePoliceReport.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                casePoliceReport.TenantId = (int)AbpSession.TenantId;
            }

            var isIcConsistent = await CheckICIsConsistent(input.ReportType, input.RegisterId, input.ComplainantIdentityNo);

            casePoliceReport.IsDataConsistent = isIcConsistent;

            await _casePoliceReportRepository.InsertAsync(casePoliceReport);

            var folderFieldName = "";

            switch (input.ReportType)
            {
                case "Police":
                    folderFieldName = EnumFolderField.PoliceReportInsurer;
                    break;
                case "Claimant":
                    folderFieldName = EnumFolderField.ClaimantPoliceReport;
                    break;
                case "ThirdParty":
                    folderFieldName = EnumFolderField.ThirdPartyPoliceReport;
                    break;
                default:
                    throw new UserFriendlyException(L("ReportTypeNotFound"));
            }
                
            casePoliceReport.ReportFileUpload = await _fileOrgService.GetBinaryObjectFromCache(input.ReportFileUploadToken, input.RegisterId, folderFieldName);
        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReports_Edit)]
        protected virtual async Task Update(CreateOrEditCasePoliceReportDto input)
        {
            var casePoliceReport = await _casePoliceReportRepository.FirstOrDefaultAsync((int)input.Id);

            var isIcConsistent = await CheckICIsConsistent(input.ReportType, input.RegisterId, input.ComplainantIdentityNo);

            casePoliceReport.IsDataConsistent = isIcConsistent;

            // TODO: remove generated summary and inconsistencies when update police report / IC/License at steps
            ObjectMapper.Map(input, casePoliceReport);

            var folderFieldName = "";

            switch (input.ReportType)
            {
                case "Police":
                    folderFieldName = EnumFolderField.PoliceReportInsurer;
                    break;
                case "Claimant":
                    folderFieldName = EnumFolderField.ClaimantPoliceReport;
                    break;
                case "ThirdParty":
                    folderFieldName = EnumFolderField.ThirdPartyPoliceReport;
                    break;
                default:
                    throw new UserFriendlyException(L("ReportTypeNotFound"));
            }
            casePoliceReport.ReportFileUpload = await _fileOrgService.GetBinaryObjectFromCache(input.ReportFileUploadToken, input.RegisterId, folderFieldName);
        }

        [AbpAuthorize(AppPermissions.Pages_CasePoliceReports_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _casePoliceReportRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_CasePoliceReports)]
        public async Task<List<CasePoliceReportMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CasePoliceReportMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }


        [AbpAuthorize(AppPermissions.Pages_CasePoliceReports_Edit)]
        public virtual async Task RemoveReportFileUploadFile(EntityDto input)
        {
            var casePoliceReport = await _casePoliceReportRepository.FirstOrDefaultAsync(input.Id);
            if (casePoliceReport == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!casePoliceReport.ReportFileUpload.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(casePoliceReport.ReportFileUpload);
            casePoliceReport.ReportFileUpload = null;
        }

        public async Task<List<CommonDropdownDto>> GetAllPoliceReportTypeDropdown()
        {
            return await _lookup_lookupRepository.GetAll()
                .Where(x => x.Group.Equals("PoliceReportType"))
                .OrderBy(x => x.Sequence)
                .Select(o => new CommonDropdownDto
                {
                    Id = (o.Id),
                    DisplayName = o == null || o.Description == null ? "" : o.Description.ToString()
                }).ToListAsync();
        }

        private async Task<bool>CheckICIsConsistent(string reportType, int registerId, string identityNo)
        {
            if(reportType != "Police")
            {

                if (identityNo == null)
                {
                    throw new UserFriendlyException("Complainant Identity Number field is required.");
                }
                else
                {
                    identityNo = Regex.Replace(identityNo, @"[\s-]", "");
                }
            }
            
            if (reportType == "Claimant")
            {
                var cip = await _caseInsuredPersonRepository.FirstOrDefaultAsync(x => x.RegisterId == registerId && x.IsOwner && x.IdenticationType == "IC");

                if(cip != null)
                {
                    var caseInsuredPersonIdentityNo = Regex.Replace(cip.IdenticationNo, @"[\s-]", "");

                    return caseInsuredPersonIdentityNo == identityNo;
                }
                else
                {
                    // adjuster have not upload IC at step 2
                    return false;
                }
            } else if(reportType == "ThirdParty")
            {
                var ctpi = await _caseThirdPartyInfoRepository.GetAll()
                    .Where(x => x.RegisterId == registerId)
                    .Join(_caseInsuredPersonRepository.GetAll(), ctpi => ctpi.CaseInsuredPersonId, cip => cip.Id, (ctpi, cip) => new { ctpi, cip })
                    .Where(x => x.cip.IsThirdParty && x.cip.IdenticationType == "IC")
                    .ToListAsync();

                if (ctpi != null)
                {
                    foreach (var item in ctpi)
                    {
                        var caseInsuredPersonIdentityNo = Regex.Replace(item.cip.IdenticationNo, @"[\s-]", "");
                        if (caseInsuredPersonIdentityNo == identityNo)
                        {
                            return true;
                        }
                    }

                    // third party in police report is not found in step 8
                    return false;
                }
                else
                {
                    // adjuster have not insert any third party at step 8
                    return false;
                }
            }

            return true;
        }
    }
}