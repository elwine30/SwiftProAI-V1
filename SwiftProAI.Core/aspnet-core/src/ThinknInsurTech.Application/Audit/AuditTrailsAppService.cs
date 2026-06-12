using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Audit.Exporting;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Audit
{
    [AbpAuthorize(AppPermissions.Pages_Administration_AuditTrails)]
    public abstract class AuditTrailsAppServiceBase : ThinknInsurTechAppServiceBase
    {
        private readonly IRepository<AuditTrail> _auditTrailRepository;
        private readonly IAuditTrailsExcelExporter _auditTrailsExcelExporter;

        public AuditTrailsAppServiceBase(IRepository<AuditTrail> auditTrailRepository, IAuditTrailsExcelExporter auditTrailsExcelExporter)
        {
            _auditTrailRepository = auditTrailRepository;
            _auditTrailsExcelExporter = auditTrailsExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetAuditTrailForViewDto>> GetAll(GetAllAuditTrailsInput input)
        {

            var filteredAuditTrails = _auditTrailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Operation.Contains(input.Filter) || e.TableName.Contains(input.Filter) || e.ChangedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OperationFilter), e => e.Operation.Contains(input.OperationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TableNameFilter), e => e.TableName.Contains(input.TableNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChangedByFilter), e => e.ChangedBy.Contains(input.ChangedByFilter))
                        .WhereIf(input.MinChangedDateFilter != null, e => e.ChangedDate >= input.MinChangedDateFilter)
                        .WhereIf(input.MaxChangedDateFilter != null, e => e.ChangedDate <= input.MaxChangedDateFilter);

            var pagedAndFilteredAuditTrails = filteredAuditTrails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var auditTrails = from o in pagedAndFilteredAuditTrails
                              select new
                              {

                                  o.Operation,
                                  o.TableName,
                                  o.ChangedBy,
                                  o.ChangedDate,
                                  Id = o.Id
                              };

            var totalCount = await filteredAuditTrails.CountAsync();

            var dbList = await auditTrails.ToListAsync();
            var results = new List<GetAuditTrailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAuditTrailForViewDto()
                {
                    AuditTrail = new AuditTrailDto
                    {

                        Operation = o.Operation,
                        TableName = o.TableName,
                        ChangedBy = o.ChangedBy,
                        ChangedDate = o.ChangedDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAuditTrailForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetAuditTrailForViewDto> GetAuditTrailForView(int id)
        {
            var auditTrail = await _auditTrailRepository.GetAsync(id);

            var output = new GetAuditTrailForViewDto { AuditTrail = ObjectMapper.Map<AuditTrailDto>(auditTrail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_AuditTrails_Edit)]
        public virtual async Task<GetAuditTrailForEditOutput> GetAuditTrailForEdit(EntityDto input)
        {
            var auditTrail = await _auditTrailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAuditTrailForEditOutput { AuditTrail = ObjectMapper.Map<CreateOrEditAuditTrailDto>(auditTrail) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditAuditTrailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_AuditTrails_Create)]
        protected virtual async Task Create(CreateOrEditAuditTrailDto input)
        {
            var auditTrail = ObjectMapper.Map<AuditTrail>(input);

            if (AbpSession.TenantId != null)
            {
                auditTrail.TenantId = (int?)AbpSession.TenantId;
            }

            await _auditTrailRepository.InsertAsync(auditTrail);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_AuditTrails_Edit)]
        protected virtual async Task Update(CreateOrEditAuditTrailDto input)
        {
            var auditTrail = await _auditTrailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, auditTrail);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_AuditTrails_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _auditTrailRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetAuditTrailsToExcel(GetAllAuditTrailsForExcelInput input)
        {

            var filteredAuditTrails = _auditTrailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Operation.Contains(input.Filter) || e.TableName.Contains(input.Filter) || e.ChangedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OperationFilter), e => e.Operation.Contains(input.OperationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TableNameFilter), e => e.TableName.Contains(input.TableNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChangedByFilter), e => e.ChangedBy.Contains(input.ChangedByFilter))
                        .WhereIf(input.MinChangedDateFilter != null, e => e.ChangedDate >= input.MinChangedDateFilter)
                        .WhereIf(input.MaxChangedDateFilter != null, e => e.ChangedDate <= input.MaxChangedDateFilter);

            var query = (from o in filteredAuditTrails
                         select new GetAuditTrailForViewDto()
                         {
                             AuditTrail = new AuditTrailDto
                             {
                                 Operation = o.Operation,
                                 TableName = o.TableName,
                                 ChangedBy = o.ChangedBy,
                                 ChangedDate = o.ChangedDate,
                                 Id = o.Id
                             }
                         });

            var auditTrailListDtos = await query.ToListAsync();

            return _auditTrailsExcelExporter.ExportToFile(auditTrailListDtos);
        }

    }
}