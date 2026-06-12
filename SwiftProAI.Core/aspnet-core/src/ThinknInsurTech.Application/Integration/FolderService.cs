using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Integration
{
    public class FolderService : IFolderService, ITransientDependency
    {
        private readonly IRepository<Folder, int> _folderRepository;
        private readonly IRepository<CaseThirdPartyInfo, int> _thirdPartyRepository;
        private readonly IRepository<CaseThirdPartyVehicle, int> _thirdPartyVehRepository;

        private readonly IHostEnvironment _env;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private static IConfigurationRoot _configuration;



        public FolderService(
            IRepository<Folder, int> folderRepository,
            IRepository<CaseThirdPartyInfo, int> thirdPartyRepository,
            IRepository<CaseThirdPartyVehicle, int> thirdPartyVehRepository,
            IHostEnvironment env,
            IAbpSession abpSession,
            IUnitOfWorkManager unitOfWorkManager,
           IAppConfigurationAccessor appConfigurationAccessor
        )
        {
            _folderRepository = folderRepository;
            _env = env;
            _abpSession = abpSession;
            _unitOfWorkManager = unitOfWorkManager;
            _thirdPartyRepository = thirdPartyRepository;
            _thirdPartyVehRepository = thirdPartyVehRepository;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public List<FolderDto> GetAll(GetAllFoldersInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return _folderRepository.GetAll()
              .WhereIf(!input.FieldFilter.IsNullOrWhiteSpace(), fol => fol.Field == input.FieldFilter)
              //This is using !CONTAINS! as ThirdPartyInfo and ThirdPartyVehicle
              //MainEntity stores both entity by appending IC number OR Vehicle Number at the end
              .WhereIf(!input.MainEntityFilter.IsNullOrWhiteSpace(), fol => fol.MainEntity.Contains(input.MainEntityFilter))
              .WhereIf(input.MainEntityIdFilter.HasValue && input.MainEntityIdFilter > 0, fol => fol.MainEntityId == input.MainEntityIdFilter)
              .Select(f => new FolderDto()
              {
                  Field = f.Field,
                  Id = f.Id,
                  MainEntity = f.MainEntity,
                  MainEntityId = (int)f.MainEntityId
              })
             .ToList();
            }


        }


        /// <summary>
        /// Retrieves folder information based on the provided registerId and stores the results in a nested dictionary format.
        /// - Key Difference between GetAll: Filters based on MainRegistration ID.
        /// </summary>
        public async Task<Dictionary<string, Dictionary<string, int>>> GetAllInDictionary(
            int registerId
        )
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {

                var thirdPartyInfoIds = await _thirdPartyRepository
                    .GetAll()
                    .Where(tp => tp.RegisterId == registerId)
                    .Select(tp => tp.Id)
                    .ToListAsync();

                var thirdPartyVehicleIds = await _thirdPartyVehRepository
                    .GetAll()
                    .Where(tv => tv.RegisterId == registerId)
                    .Select(tv => tv.Id)
                    .ToListAsync();

                var query = _folderRepository.GetAll();

                if (thirdPartyInfoIds.Any() || thirdPartyVehicleIds.Any())
                {
                    query = query.Where(f =>
                        (
                            thirdPartyInfoIds.Count != 0
                            && f.MainEntity.Contains(FolderConsts.ThirdPartyInfoMainEntity)
                            && thirdPartyInfoIds.Contains((int)f.MainEntityId)
                        )
                        || (
                            thirdPartyVehicleIds.Count != 0
                            && f.MainEntity.Contains(FolderConsts.ThirdPartyVehicleMainEntity)
                            && thirdPartyVehicleIds.Contains((int)f.MainEntityId)
                        )
                        || f.MainEntityId == null
                    );
                }
                else
                {
                    query = query.Where(f => f.MainEntityId == null);
                }

                // Execute the query and build the dictionary
                var allFolders = await query
                    .GroupBy(f => f.MainEntity)
                    .ToDictionaryAsync(g => g.Key, g => g.ToDictionary(f => f.Field, f => f.Id));

                return allFolders;
            }
        }

        public List<FolderDto> GetAllByMainEntityAndId(string mainEntity, int mainEntityId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                return _folderRepository
                   .GetAll()
                   .Where(f => f.MainEntity.Contains(mainEntity))
                   .Where(f => f.MainEntityId == mainEntityId)
                   .Select(f => new FolderDto()
                   {
                       Field = f.Field,
                       Id = f.Id,
                       MainEntity = f.MainEntity,
                       MainEntityId = (int)f.MainEntityId
                   })
                   .ToList();
            }


        }

        public async Task<List<FolderDto>> GetAllByMainEntityAndIdAsync(string mainEntity, int mainEntityId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                return await _folderRepository
                   .GetAll()
                   .Where(f => f.MainEntity.Contains(mainEntity))
                   .Where(f => f.MainEntityId == mainEntityId)
                   .Select(f => new FolderDto()
                   {
                       Field = f.Field,
                       Id = f.Id,
                       MainEntity = f.MainEntity,
                       MainEntityId = (int)f.MainEntityId
                   })
                   .ToListAsync();
            }


        }

        public async Task DeleteFolder(int folderId, int registerId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var folder = await _folderRepository.GetAsync(folderId);
                if (folder == null)
                {
                    throw new UserFriendlyException($"Error: Folder with Id {folderId} is not found in Folder table.");
                }

                var path = await GenerateDirectoryByFolderId(folderId, registerId.ToString());

                // Check if the directory exists before attempting to delete it
                if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path, true);
                    }
                    catch (Exception ex)
                    {
                        throw new UserFriendlyException($"Error deleting directory '{path}': {ex.Message}");
                    }
                }

                await _folderRepository.DeleteAsync(folder);
            }
        }
        public async Task<string> GenerateDirectory(string folderField, string caseNo)
        {
            // This GenerateDirectory function uses the folderField. Please make sure an entry of the corresponding FolderField exists in the Folder table in Database.
            {
                using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                {
                    var path = "";

                    var folder = await _folderRepository.FirstOrDefaultAsync(w =>
                        w.Field.Equals(folderField)
                    );

                    if (folder == null)
                    {
                        throw new UserFriendlyException(
                            $"Error: Folder with field {folderField} is not found in Folder table."
                        );
                    }

                    var root = Path.Combine(Directory.GetDirectoryRoot(_env.ContentRootPath), _configuration.GetSection("Folder")["root"]);

                    path = Path.Combine(
                        root,
                        _abpSession.TenantId.ToString(),
                        caseNo,
                        folder.MainEntity,
                        folder.Field
                    );

                    return path;
                }
            }
        }

        public async Task<int> CreateByMainEntityAndField(
            string mainEntity,
            string field,
            int mainEntityId
        )
        {
            var folder = new Folder
            {
                MainEntity = mainEntity,
                Field = field,
                MainEntityId = mainEntityId,
                CreationTime = DateTime.Now,
                CreatorUserId = _abpSession.UserId,
                TenantId = _abpSession.GetTenantId(),
            };
            //Applied OUID
            if (_abpSession.GetCurrentOUId() != null)
            {
                folder.OrganizationUnitId = _abpSession.GetCurrentOUId();
            }
            return _folderRepository.InsertAndGetId(folder);
        }

        public async Task<string> GenerateDirectoryByFolderId(int folderId, string caseNo)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var path = "";

                var folder = await _folderRepository.GetAsync(folderId);

                if (folder == null)
                {
                    throw new UserFriendlyException(
                        $"Error: Folder with Id {folderId} is not found in Folder table."
                    );
                }

                var root = Path.Combine(Directory.GetDirectoryRoot(_env.ContentRootPath), _configuration.GetSection("Folder")["root"]);

                path = Path.Combine(
                    root,
                    _abpSession.TenantId.ToString(),
                    caseNo,
                    folder.MainEntity,
                    folder.Field
                );

                return path;
            }


        }

        public async Task MoveIntoFolderAsync(string sourcePath, string targetPath)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
                    {
                        throw new FileNotFoundException("Source path does not exist.");
                    }

                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    string sourceName = Path.GetFileName(sourcePath);

                    string destinationPath = Path.Combine(targetPath, sourceName);

                    if (File.Exists(sourcePath))
                    {
                        // Move the file
                        File.Move(sourcePath, destinationPath);
                    }
                    else if (Directory.Exists(sourcePath))
                    {
                        // Move the directory
                        Directory.Move(sourcePath, destinationPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error moving '{sourcePath}' to '{targetPath}': {ex.Message}"
                    );
                }
            });
        }


        #region ThirdPartyInfo


        //public List<FolderDto> GetAllThirdPartyFolder(int thirdPartyId, string idNumber)
        //{
        //    var mainEntity = $"ThirdPartyInfo - {idNumber}";

        //    var folderIds =
        //        _folderRepository
        //            .GetAll()
        //            .Where(f => f.MainEntity == mainEntity && f.MainEntityId == thirdPartyId)
        //            .Select(f => new FolderDto()
        //            {
        //                Field = f.Field,
        //                Id = f.Id,
        //                MainEntity = f.MainEntity,
        //                MainEntityId = (int)f.MainEntityId
        //            })
        //            .ToList() ?? null;

        //    return folderIds;
        //}








        #endregion
    }
}
