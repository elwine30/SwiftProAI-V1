using ThinknInsurTech.Audit;

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
using Abp.Organizations;

namespace ThinknInsurTech.Audit
{
    [AbpAuthorize(AppPermissions.Pages_Administration_AuditEntries)]
    public class AuditEntriesAppService : ThinknInsurTechAppServiceBase, IAuditEntriesAppService
    {
        private readonly IRepository<AuditEntry> _auditEntryRepository;
        private readonly IAuditEntriesExcelExporter _auditEntriesExcelExporter;
        private readonly IRepository<AuditTrail, int> _lookup_auditTrailRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;

        public AuditEntriesAppService(IRepository<AuditEntry> auditEntryRepository, IAuditEntriesExcelExporter auditEntriesExcelExporter, IRepository<AuditTrail, int> lookup_auditTrailRepository, IRepository<OrganizationUnit, long> lookupOrganizationUnitRepository)
        {
            _auditEntryRepository = auditEntryRepository;
            _auditEntriesExcelExporter = auditEntriesExcelExporter;
            _lookup_auditTrailRepository = lookup_auditTrailRepository;
            _lookup_organizationUnitRepository = lookupOrganizationUnitRepository;
        }

        public virtual async Task<PagedResultDto<GetAuditEntryForViewDto>> GetAll(GetAllAuditEntriesInput input)
        {

            var filteredAuditEntries = _auditEntryRepository.GetAll()
                        .Include(e => e.AuditTrailFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FieldName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TableName), e => e.AuditTrailFk.TableName.Contains(input.TableName))
                        .WhereIf(input.OrganizationUnitId.HasValue, e => e.AuditTrailFk.OrganizationUnit.Value == input.OrganizationUnitId);

            var pagedAndFilteredAuditEntries = filteredAuditEntries
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var auditEntries = from o in pagedAndFilteredAuditEntries
                               join o1 in _lookup_auditTrailRepository.GetAll() on o.AuditTrailId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()
                               join ou in _lookup_organizationUnitRepository.GetAll() on s1.OrganizationUnit equals ou.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new
                               {
                                   o.FieldName,
                                   o.OldValue,
                                   o.NewValue,
                                   Id = o.Id,
                                   s1.Operation,
                                   s1.TableName,
                                   s1.ChangedBy,
                                   s2.DisplayName,
                                   s1.ChangedDate
                               };

            var totalCount = await filteredAuditEntries.CountAsync();

            var dbList = await auditEntries.ToListAsync();
            var results = new List<GetAuditEntryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAuditEntryForViewDto()
                {
                    AuditEntry = new AuditEntryDto
                    {
                        FieldName = o.FieldName,
                        OldValue = o.OldValue,
                        NewValue = o.NewValue,
                        Id = o.Id,
                        TableName = o.TableName,
                        Method = o.Operation,
                        ChangedBy = o.ChangedBy,
                        OrganizationUnit = o.DisplayName,
                        ChangedDate = o.ChangedDate
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAuditEntryForViewDto>(
                totalCount,
                results
            );
        }

        public virtual async Task<FileDto> GetAuditEntriesToExcel(GetAllAuditEntriesForExcelInput input)
        {

            var filteredAuditEntries = _auditEntryRepository.GetAll()
                        .Include(e => e.AuditTrailFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FieldName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrEmpty(input.TableName), e => e.AuditTrailFk.TableName.Contains(input.TableName));

            var query = (from o in filteredAuditEntries
                         join o1 in _lookup_auditTrailRepository.GetAll() on o.AuditTrailId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetAuditEntryForViewDto()
                         {
                             AuditEntry = new AuditEntryDto
                             {
                                 FieldName = o.FieldName,
                                 OldValue = o.OldValue,
                                 NewValue = o.NewValue,
                                 Id = o.Id
                             },
                         });

            var auditEntryListDtos = await query.ToListAsync();

            return _auditEntriesExcelExporter.ExportToFile(auditEntryListDtos);
        }

        public Task<GetAuditEntryForViewDto> GetAuditEntryForView(int id)
        {
            throw new NotImplementedException();
        }
    }
}