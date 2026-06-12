using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyVehicles)]
    public class CaseThirdPartyVehiclesAppService : ThinknInsurTechAppServiceBase, ICaseThirdPartyVehiclesAppService
    {
        private readonly IRepository<CaseThirdPartyVehicle> _caseThirdPartyVehicleRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;
        private readonly FileOrgService _fileOrgService;
        private readonly FolderService _folderService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;


        public CaseThirdPartyVehiclesAppService(IRepository<CaseThirdPartyVehicle> caseThirdPartyVehicleRepository, FileOrgService fileOrgService, FolderService folderService,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository, IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _caseThirdPartyVehicleRepository = caseThirdPartyVehicleRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_insuranceCompanyRepository = lookup_insuranceCompanyRepository;
            _fileOrgService = fileOrgService;
            _folderService = folderService;
            _unitOfWorkManager = unitOfWorkManager;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
        }

        public virtual async Task<PagedResultDto<GetCaseThirdPartyVehicleForViewDto>> GetAll(GetAllCaseThirdPartyVehiclesInput input)
        {

            int registerId = Convert.ToInt32(input.RegisterIdFilter);
            var filteredCaseThirdPartyVehicles = _caseThirdPartyVehicleRepository.GetAll()
                        .Include(e => e.RegisterFk)
                        .Include(e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id == registerId);

            var pagedAndFilteredCaseThirdPartyVehicles = filteredCaseThirdPartyVehicles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseThirdPartyVehicles = from o in pagedAndFilteredCaseThirdPartyVehicles
                                         join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_insuranceCompanyRepository.GetAll() on o.CompanyId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new
                                         {
                                             o.CompanyId,
                                             o.RegisterId,
                                             o.VehicleNo,
                                             o.RegisteredOwner,
                                             o.VehicleMake,
                                             o.VehicleYear,
                                             o.PolicyNo,
                                             o.TypeCover,
                                             o.CoverStartDate,
                                             o.CoverEndDate,
                                             o.Id,
                                             MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo,
                                             CompanyName = s2 == null || s2.Name == null ? "" : s2.Name
                                         };

            var totalCount = await filteredCaseThirdPartyVehicles.CountAsync();

            var dbList = await caseThirdPartyVehicles.ToListAsync();
            var results = new List<GetCaseThirdPartyVehicleForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseThirdPartyVehicleForViewDto()
                {
                    CaseThirdPartyVehicle = new CaseThirdPartyVehicleDto
                    {
                        RegisterId = o.RegisterId,
                        CompanyId = o.CompanyId,
                        VehicleNo = o.VehicleNo,
                        RegisteredOwner = o.RegisteredOwner,
                        VehicleMake = o.VehicleMake,
                        VehicleYear = o.VehicleYear,
                        PolicyNo = o.PolicyNo,
                        TypeCover = o.TypeCover,
                        CoverStartDate = o.CoverStartDate,
                        CoverEndDate = o.CoverEndDate,
                        Id = o.Id,
                    },
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseThirdPartyVehicleForViewDto>(
                totalCount,
                results
            );


        }

        public async Task<PagedResultDto<GetCaseThirdPartyVehicleForViewDto>> GetAllForView(GetAllCaseThirdPartyVehiclesInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;
                int registerId = Convert.ToInt32(input.RegisterIdFilter);
                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == registerId).Select(f => f.RegisterId).FirstOrDefault();

                var filteredCaseThirdPartyVehicles = _caseThirdPartyVehicleRepository.GetAll()
                            .Include(e => e.RegisterFk)
                            .Include(e => e.CompanyFk)
                            .Where(f => f.RegisterId == assignedRegisterId);

                var pagedAndFilteredCaseThirdPartyVehicles = filteredCaseThirdPartyVehicles
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var caseThirdPartyVehicles = from o in pagedAndFilteredCaseThirdPartyVehicles
                                             join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                             from s1 in j1.DefaultIfEmpty()

                                             join o2 in _lookup_insuranceCompanyRepository.GetAll() on o.CompanyId equals o2.Id into j2
                                             from s2 in j2.DefaultIfEmpty()

                                             select new
                                             {
                                                 o.CompanyId,
                                                 o.RegisterId,
                                                 o.VehicleNo,
                                                 o.RegisteredOwner,
                                                 o.VehicleMake,
                                                 o.VehicleYear,
                                                 o.PolicyNo,
                                                 o.TypeCover,
                                                 o.CoverStartDate,
                                                 o.CoverEndDate,
                                                 o.Id,
                                                 MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo,
                                                 CompanyName = s2 == null || s2.Name == null ? "" : s2.Name
                                             };

                var totalCount = await filteredCaseThirdPartyVehicles.CountAsync();

                var dbList = await caseThirdPartyVehicles.ToListAsync();
                var results = new List<GetCaseThirdPartyVehicleForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCaseThirdPartyVehicleForViewDto()
                    {
                        CaseThirdPartyVehicle = new CaseThirdPartyVehicleDto
                        {
                            RegisterId = o.RegisterId,
                            CompanyId = o.CompanyId,
                            VehicleNo = o.VehicleNo,
                            RegisteredOwner = o.RegisteredOwner,
                            VehicleMake = o.VehicleMake,
                            VehicleYear = o.VehicleYear,
                            PolicyNo = o.PolicyNo,
                            TypeCover = o.TypeCover,
                            CoverStartDate = o.CoverStartDate,
                            CoverEndDate = o.CoverEndDate,
                            Id = o.Id,
                        },
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetCaseThirdPartyVehicleForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        // TODO
        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyVehicles_Edit)]
        public virtual async Task<GetCaseThirdPartyVehicleForEditOutput> GetCaseThirdPartyVehicleForEdit(EntityDto input)
        {
            var caseThirdPartyVehicle = _caseThirdPartyVehicleRepository.GetAll().Where(w => w.Id.Equals(input.Id)).FirstOrDefault();

            var output = new GetCaseThirdPartyVehicleForEditOutput { CaseThirdPartyVehicle = ObjectMapper.Map<CreateOrEditCaseThirdPartyVehicleDto>(caseThirdPartyVehicle) };

            if (caseThirdPartyVehicle != null)
            {
                if (caseThirdPartyVehicle.CompanyId.HasValue)
                {
                    var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync(output.CaseThirdPartyVehicle.CompanyId.Value);
                    output.CompanyName = _lookupCompany.Name;
                }

                if (caseThirdPartyVehicle.Id > 0)
                {
                    var folders = _folderService.GetAllByMainEntityAndId(caseThirdPartyVehicle.VehicleNo, (int)caseThirdPartyVehicle.Id);
                    var tpvCarGrantFolderId = folders
                     .Where(x => x.Field == FolderConsts.ThirdPartyVehicleFields[0])
                     .Select(x => x.Id)
                     .FirstOrDefault();

                    var tpvDetailFolderId = folders
                        .Where(x => x.Field == FolderConsts.ThirdPartyVehicleFields[1])
                        .Select(x => x.Id)
                        .FirstOrDefault();

                    output.CaseThirdPartyVehicle.TPVDetails = _fileOrgService.GetMetadataByFolderIdAndCaseNo((int)tpvDetailFolderId, output.CaseThirdPartyVehicle.RegisterId).FirstOrDefault();

                    if (output.CaseThirdPartyVehicle.DriverCarGrant != null)
                        output.CaseThirdPartyVehicle.TPVCarGrant = _fileOrgService.GetMetadataByReference((Guid)caseThirdPartyVehicle.DriverCarGrant);
                }

            }
            return output;
        }

        protected async Task checkValidationForThirdPartyVehicle(string vehicleNo, int registerId, int? id)
        {

            var isVehicleRegistered = await _caseThirdPartyVehicleRepository.GetAll()
             .WhereIf(id != null && id > 0, f => f.Id != id)
             .Where(f => f.RegisterId == registerId && f.VehicleNo == vehicleNo)
             .AnyAsync();

            if (isVehicleRegistered)
            {
                throw new UserFriendlyException("Car Plate already registered under this case");

            }
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseThirdPartyVehicleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyVehicles_Create)]
        protected virtual async Task Create(CreateOrEditCaseThirdPartyVehicleDto input)
        {
            await checkValidationForThirdPartyVehicle(input.VehicleNo, input.RegisterId, null);
            await ValidateCoverDateRange(input.CoverStartDate, input.CoverEndDate);

            var caseThirdPartyVehicle = ObjectMapper.Map<CaseThirdPartyVehicle>(input);
            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseThirdPartyVehicle.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseThirdPartyVehicle.TenantId = (int)AbpSession.TenantId;
            }

            var caseCreated = await _caseThirdPartyVehicleRepository.InsertAndGetIdAsync(caseThirdPartyVehicle);

            if (caseCreated != null && caseCreated > 0)
            {

                //File saving section
                using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                {

                    //Folder creation section
                    var tpvCarGrantFolderId = 0;
                    var tpvDetailFolderId = 0;

                    foreach (var subFolder in FolderConsts.ThirdPartyVehicleFields)
                    {
                        var tempFolder = await _folderService.CreateByMainEntityAndField($"{FolderConsts.ThirdPartyVehicleMainEntity} - {input.VehicleNo}", subFolder, caseCreated);
                        if (subFolder == FolderConsts.ThirdPartyVehicleFields[1])
                        {
                            tpvDetailFolderId = tempFolder;
                        }
                        else if (subFolder == FolderConsts.ThirdPartyVehicleFields[0])
                        {
                            tpvCarGrantFolderId = tempFolder;
                        }
                    }
                    //Car Grant
                    if (input.TPVCarGrantToken != null)
                    {
                        caseThirdPartyVehicle.DriverCarGrant = await _fileOrgService.GetBinaryObjectFromCacheToId(input.TPVCarGrantToken, (int)input.RegisterId, tpvCarGrantFolderId);
                    }
                    //TPV Detail Files
                    if (input.TPVDetailsToken != null)
                    {
                        await _fileOrgService.GetBinaryObjectFromCacheToId(input.TPVDetailsToken, (int)input.RegisterId, tpvDetailFolderId);

                    }

                }
            }
            await _caseThirdPartyVehicleRepository.UpdateAsync(caseThirdPartyVehicle);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyVehicles_Edit)]
        protected virtual async Task Update(CreateOrEditCaseThirdPartyVehicleDto input)
        {
            await checkValidationForThirdPartyVehicle(input.VehicleNo, input.RegisterId, input.Id);

            await ValidateCoverDateRange(input.CoverStartDate, input.CoverEndDate);

            var caseThirdPartyVehicle = await _caseThirdPartyVehicleRepository.FirstOrDefaultAsync((int)input.Id);
            if (input.VehicleNo != caseThirdPartyVehicle.VehicleNo)
            {
                await ChangeDirectory(input.RegisterId, (int)input.Id, input.VehicleNo, caseThirdPartyVehicle.VehicleNo);
            }
            ObjectMapper.Map(input, caseThirdPartyVehicle);
            var folders = _folderService.GetAllByMainEntityAndId(FolderConsts.ThirdPartyVehicleMainEntity, (int)input.Id);

            var tpvCarGrantFolderId = folders
             .Where(x => x.Field == FolderConsts.ThirdPartyVehicleFields[0])
             .Select(x => x.Id)
             .FirstOrDefault();

            var tpvDetailFolderId = folders
                .Where(x => x.Field == FolderConsts.ThirdPartyVehicleFields[1])
                .Select(x => x.Id)
                .FirstOrDefault();

            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if (!input.TPVCarGrantToken.IsNullOrEmpty())
                    caseThirdPartyVehicle.DriverCarGrant = await _fileOrgService.GetBinaryObjectFromCacheToId(input.TPVCarGrantToken, input.RegisterId, tpvCarGrantFolderId);

                if (input.TPVDetailsToken != null)
                {
                    await _fileOrgService.GetBinaryObjectFromCacheToId(input.TPVDetailsToken, (int)input.RegisterId, tpvDetailFolderId);
                }
            }
            await _caseThirdPartyVehicleRepository.UpdateAsync(caseThirdPartyVehicle);



        }

        protected async Task ChangeDirectory(int registerId, int id, string newVehicleNo, string oldVehicleNo)
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                //Process delete
                var folders = await _folderService.GetAllByMainEntityAndIdAsync($"{FolderConsts.ThirdPartyVehicleMainEntity} - {oldVehicleNo}", id);
                await _fileOrgService.DeleteFileByMainEntityAndMainEntityID(FolderConsts.ThirdPartyVehicleMainEntity, id);

                foreach (var folder in folders)
                {
                    await _folderService.DeleteFolder(folder.Id, registerId);
                }

                //Process Create
                foreach (var subFolder in FolderConsts.ThirdPartyVehicleFields)
                {
                    var tempFolder = await _folderService.CreateByMainEntityAndField($"{FolderConsts.ThirdPartyVehicleMainEntity} - {newVehicleNo}", subFolder, id);


                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyVehicles_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _fileOrgService.DeleteFileByMainEntityAndMainEntityID("ThirdPartyVehicle", input.Id);
            await _caseThirdPartyVehicleRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseThirdPartyVehicles)]
        public async Task<List<CaseThirdPartyVehicleMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CaseThirdPartyVehicleMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }

        private async Task ValidateCoverDateRange(DateTime? coverStartDate, DateTime? coverEndDate)
        {
            if (coverEndDate <= coverStartDate)
            {
                throw new UserFriendlyException("Cover start date cannot be later than end date");
            }
        }

        [AbpAuthorize(AppPermissions.Pages_InsuredPersons_Edit)]
        public virtual async Task RemoveDriverCarGrantFile(EntityDto input)
        {
            var caseThirdPartyVehicle = await _caseThirdPartyVehicleRepository.FirstOrDefaultAsync(input.Id);
            if (caseThirdPartyVehicle == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!caseThirdPartyVehicle.DriverCarGrant.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _fileOrgService.DeleteFileByReference(caseThirdPartyVehicle.DriverCarGrant.Value);
            caseThirdPartyVehicle.DriverCarGrant = null;
        }


    }
}