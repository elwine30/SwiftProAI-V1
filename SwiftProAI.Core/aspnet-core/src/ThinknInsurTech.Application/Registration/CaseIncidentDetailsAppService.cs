using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails)]
    public class CaseIncidentDetailsAppService : ThinknInsurTechAppServiceBase, ICaseIncidentDetailsAppService
    {
        private readonly IRepository<CaseIncidentDetail> _caseIncidentDetailRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IFileOrgService _fileOrgService;
        private readonly IRepository<FileOrg, int> _lookup_fileOrgRepository;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;


        public CaseIncidentDetailsAppService(IRepository<CaseIncidentDetail> caseIncidentDetailRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, ITempFileCacheManager tempFileCacheManager
            , IBinaryObjectManager binaryObjectManager, IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository, IRepository<FileOrg, int> lookup_fileOrgRepository, IFileOrgService fileOrgService, IUnitOfWorkManager unitOfWorkManager)
        {
            _caseIncidentDetailRepository = caseIncidentDetailRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_fileOrgRepository = lookup_fileOrgRepository;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _fileOrgService = fileOrgService;
            _unitOfWorkManager = unitOfWorkManager;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;

        }

        public async Task<GetCaseIncidentDetailForEditOutput> GetOneData(int mainId)
        {
            var caseIncidentDetail = _caseIncidentDetailRepository.GetAll()
                .FirstOrDefault(w => w.RegisterId.Equals(mainId));

            var output = new GetCaseIncidentDetailForEditOutput
            {
                CaseIncidentDetail = ObjectMapper.Map<CreateOrEditCaseIncidentDetailDto>(caseIncidentDetail)
            };

            if (caseIncidentDetail != null)
            {
                var file = await _lookup_fileOrgRepository.FirstOrDefaultAsync(f => f.MainRegistrationId.Equals(mainId));
                if (file != null)
                {
                    output.CircumstancesFileUploadFileName = file.FileName;
                }
            }

            return output;
        }

        public virtual async Task<GetCaseIncidentDetailForViewDto> GetCaseIncidentDetailForView(int id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == id).Select(f => f.RegisterId).FirstOrDefault();
                var caseIncidentDetail = _caseIncidentDetailRepository.GetAll().FirstOrDefault(x => x.RegisterId == assignedRegisterId);

                var output = new GetCaseIncidentDetailForViewDto
                {
                    CaseIncidentDetail = ObjectMapper.Map<CaseIncidentDetailDto>(caseIncidentDetail)
                };

                if (caseIncidentDetail != null)
                {
                    var file = await _lookup_fileOrgRepository.FirstOrDefaultAsync(f => f.MainRegistrationId.Equals(id));
                    if (file != null)
                    {
                        output.CaseIncidentDetail.CircumstancesFileUploadFileName = file.FileName;
                    }
                }

                return output;
            }
        }


        [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails_Edit)]
        public virtual async Task<GetCaseIncidentDetailForEditOutput> GetCaseIncidentDetailForEdit(EntityDto input)
        {
            //var caseIncidentDetail = _caseIncidentDetailRepository.GetAll().Where(w => w.RegisterId.Equals(input.Id)).FirstOrDefault();

            //var output = new GetCaseIncidentDetailForEditOutput { CaseIncidentDetail = ObjectMapper.Map<CreateOrEditCaseIncidentDetailDto>(caseIncidentDetail) };
            //if (caseIncidentDetail != null)
            //{
            //    output.CircumstancesFileUploadFileName = await GetBinaryFileName(caseIncidentDetail.CircumstancesFileUpload);
            //}
            //return output;

            var caseIncidentDetail = _caseIncidentDetailRepository.GetAll()
               .FirstOrDefault(w => w.RegisterId.Equals(input.Id));

            var output = new GetCaseIncidentDetailForEditOutput
            {
                CaseIncidentDetail = ObjectMapper.Map<CreateOrEditCaseIncidentDetailDto>(caseIncidentDetail)
            };

            if (caseIncidentDetail != null && caseIncidentDetail.CircumstancesFileUpload != null)
            {
                output.CircumstancesFileUploadFileName = await _fileOrgService.GetBinaryFileName(caseIncidentDetail.CircumstancesFileUpload);
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseIncidentDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails_Create)]
        protected virtual async Task Create(CreateOrEditCaseIncidentDetailDto input)
        {
            if(!ValidateIncidentDateRange(input.TimeFrom, input.TimeTo))
            {
                throw new UserFriendlyException("Time To must be greater than Time From");
            }
            var caseIncidentDetail = ObjectMapper.Map<CaseIncidentDetail>(input);
            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseIncidentDetail.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseIncidentDetail.TenantId = (int)AbpSession.TenantId;
            }

            await _caseIncidentDetailRepository.InsertAsync(caseIncidentDetail);
            caseIncidentDetail.CircumstancesFileUpload = await _fileOrgService.GetBinaryObjectFromCache(input.CircumstancesFileUploadToken, input.RegisterId, "INC-DET");

        }

        [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails_Edit)]
        protected virtual async Task Update(CreateOrEditCaseIncidentDetailDto input)
        {
            if (!ValidateIncidentDateRange(input.TimeFrom, input.TimeTo))
            {
                throw new UserFriendlyException("Time To must be greater than Time From");
            }
            var caseIncidentDetail = await _caseIncidentDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseIncidentDetail);
            caseIncidentDetail.CircumstancesFileUpload = await _fileOrgService.GetBinaryObjectFromCache(input.CircumstancesFileUploadToken, input.RegisterId, "INC-DET");

        }

        [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            var deleteCase = _caseIncidentDetailRepository.Get(input.Id);
            if (deleteCase.CircumstancesFileUpload != null)
            {
                await _fileOrgService.DeleteFileByReference(deleteCase.CircumstancesFileUpload);
            }
            await _caseIncidentDetailRepository.DeleteAsync(input.Id);

        }
        [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails)]
        public async Task<List<CaseIncidentDetailMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CaseIncidentDetailMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }

        protected virtual async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile.Id;
        }

        protected virtual async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

        [AbpAuthorize(AppPermissions.Pages_CaseIncidentDetails_Edit)]
        public virtual async Task RemoveCircumstancesFileUploadFile(EntityDto input)
        {
            var caseIncidentDetail = await _caseIncidentDetailRepository.FirstOrDefaultAsync(input.Id);
            if (caseIncidentDetail == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!caseIncidentDetail.CircumstancesFileUpload.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(caseIncidentDetail.CircumstancesFileUpload.Value);
            caseIncidentDetail.CircumstancesFileUpload = null;
        }

        private static bool ValidateIncidentDateRange(DateTime TimeFrom, DateTime TimeTo)
        {


            return TimeTo >= TimeFrom;
        }

    }
}