using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Integration;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_Folders)]
    public class FoldersAppService : ThinknInsurTechAppServiceBase, IFoldersAppService
    {
        private readonly IRepository<Folder> _folderRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IFolderService _folderService;


        public FoldersAppService(IRepository<Folder> folderRepository, IUnitOfWorkManager unitOfWorkManager, IFolderService folderService)
        {
            _folderRepository = folderRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _folderService = folderService;
        }

        public async Task<Dictionary<string, Dictionary<string, int>>> GetAllInDictionary(int registerId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var allFolders = new Dictionary<string, Dictionary<string, int>>();
                allFolders = await _folderService.GetAllInDictionary(registerId);
                return allFolders;
            }

        }

        public virtual async Task<PagedResultDto<GetFolderForViewDto>> GetAll(GetAllFoldersInput input)
        {

            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var filteredFolders = _folderRepository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.MainEntity.Contains(input.Filter) || e.Field.Contains(input.Filter))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.MainEntityFilter), e => e.MainEntity.Contains(input.MainEntityFilter))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.FieldFilter), e => e.Field.Contains(input.FieldFilter));

                var pagedAndFilteredFolders = filteredFolders
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var folders = from o in pagedAndFilteredFolders
                              select new
                              {

                                  o.MainEntity,
                                  o.Field,
                                  Id = o.Id
                              };

                var totalCount = await filteredFolders.CountAsync();

                var dbList = await folders.ToListAsync();
                var results = new List<GetFolderForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetFolderForViewDto()
                    {
                        Folder = new FolderDto
                        {

                            MainEntity = o.MainEntity,
                            Field = o.Field,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetFolderForViewDto>(
                    totalCount,
                    results
                );
            }

        }

        public virtual async Task<GetFolderForViewDto> GetFolderForView(int id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var folder = await _folderRepository.GetAsync(id);

                var output = new GetFolderForViewDto { Folder = ObjectMapper.Map<FolderDto>(folder) };

                return output;
            }

        }



        //public virtual async Task CreateOrEdit(CreateOrEditFolderDto input)
        //{
        //    if (input.Id == null)
        //    {
        //        await Create(input);
        //    }
        //    else
        //    {
        //        await Update(input);
        //    }
        //}

        //[AbpAuthorize(AppPermissions.Pages_Folders_Create)]
        //protected virtual async Task Create(CreateOrEditFolderDto input)
        //{
        //    var folder = ObjectMapper.Map<Folder>(input);

        //    if (AbpSession.TenantId != null)
        //    {
        //        folder.TenantId = (int?)AbpSession.TenantId;
        //    }

        //    await _folderRepository.InsertAsync(folder);

        //}

        //[AbpAuthorize(AppPermissions.Pages_Folders_Edit)]
        //protected virtual async Task Update(CreateOrEditFolderDto input)
        //{
        //    var folder = await _folderRepository.FirstOrDefaultAsync((int)input.Id);
        //    ObjectMapper.Map(input, folder);

        //}

        //[AbpAuthorize(AppPermissions.Pages_Folders_Delete)]
        //public virtual async Task Delete(EntityDto input)
        //{
        //    await _folderRepository.DeleteAsync(input.Id);
        //}

    }
}