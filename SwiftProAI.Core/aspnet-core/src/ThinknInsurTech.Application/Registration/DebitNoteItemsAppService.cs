using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_DebitNoteItems)]
    public class DebitNoteItemsAppService : ThinknInsurTechAppServiceBase, IDebitNoteItemsAppService
    {
        private readonly IRepository<DebitNoteItem> _debitNoteItemRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;

        public DebitNoteItemsAppService(IRepository<DebitNoteItem> debitNoteItemRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository)
        {
            _debitNoteItemRepository = debitNoteItemRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;

        }

        public virtual async Task<PagedResultDto<GetDebitNoteItemForViewDto>> GetAll(GetAllDebitNoteItemsInput input)
        {

            var filteredDebitNoteItems = _debitNoteItemRepository.GetAll()
                        .Include(e => e.RegisterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id.ToString() == input.RegisterIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemTypeFilter), e => e.ItemType.Contains(input.ItemTypeFilter));

            var pagedAndFilteredDebitNoteItems = filteredDebitNoteItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var debitNoteItems = from o in pagedAndFilteredDebitNoteItems
                                 join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 select new
                                 {
                                     o.RegisterId,
                                     o.ItemType,
                                     o.Remark,
                                     o.Amount,
                                     o.Id,
                                 };

            var totalCount = await filteredDebitNoteItems.CountAsync();

            var dbList = await debitNoteItems.ToListAsync();
            var results = new List<GetDebitNoteItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDebitNoteItemForViewDto()
                {
                    DebitNoteItem = new DebitNoteItemDto
                    {
                        RegisterId = o.RegisterId,
                        ItemType = o.ItemType,
                        Remark = o.Remark,
                        Amount = o.Amount,
                        Id = o.Id,
                    },
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDebitNoteItemForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<List<DebitNoteItemDto>> GetDebitNoteItemByRegisterId(EntityDto input)
        {

            var filteredDebitNoteItems = _debitNoteItemRepository.GetAll()
                        .Where(w => w.RegisterId.Equals(input.Id));

            var debitNoteItems = from o in filteredDebitNoteItems
                                 select new
                                 {
                                     o.ItemType,
                                     o.Amount,
                                     o.Remark,
                                     o.Id,
                                 };

            var dbList = await debitNoteItems.ToListAsync();
            var results = new List<DebitNoteItemDto>();

            foreach (var o in dbList)
            {
                var res = new DebitNoteItemDto()
                {

                    ItemType = o.ItemType,
                    Amount = o.Amount,
                    Remark = o.Remark,
                    Id = o.Id,

                };

                results.Add(res);
            }

            return results;

        }


        public virtual async Task<List<DebitNoteItemAmountDto>> GetDebitNoteItemAmountsByRegisterId(EntityDto input)
        {

            var filteredDebitNoteItems = _debitNoteItemRepository.GetAll()
                        .Where(w => w.RegisterId.Equals(input.Id));

            var debitNoteItems = from o in filteredDebitNoteItems
                                 select new
                                 {
                                     o.ItemType,
                                     o.Amount,
                                     o.Id,
                                 };

            var dbList = await debitNoteItems.ToListAsync();
            var results = new List<DebitNoteItemAmountDto>();

            foreach (var o in dbList)
            {
                var res = new DebitNoteItemAmountDto()
                {

                    ItemType = o.ItemType,
                    Amount = o.Amount,
                    Id = o.Id,

                };

                results.Add(res);
            }

            return results;

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteItems_Edit)]
        public virtual async Task<GetDebitNoteItemForEditOutput> GetDebitNoteItemForEdit(EntityDto input)
        {

            var debitNoteItem = _debitNoteItemRepository.GetAll().Where(w => w.Id.Equals(input.Id)).FirstOrDefault();
            var output = new GetDebitNoteItemForEditOutput { DebitNoteItem = ObjectMapper.Map<CreateOrEditDebitNoteItemDto>(debitNoteItem) };


            var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.DebitNoteItem.RegisterId);
            output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDebitNoteItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DebitNoteItems_Create, AppPermissions.Pages_DebitNoteItems_Create_Adjuster, AppPermissions.Pages_DebitNoteItems_Create_Admin)]
        protected virtual async Task Create(CreateOrEditDebitNoteItemDto input)
        {
            var debitNoteItem = ObjectMapper.Map<DebitNoteItem>(input);
            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                debitNoteItem.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                debitNoteItem.TenantId = (int)AbpSession.TenantId;
            }

            await _debitNoteItemRepository.InsertAsync(debitNoteItem);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteItems_Edit)]
        protected virtual async Task Update(CreateOrEditDebitNoteItemDto input)
        {
            var debitNoteItem = await _debitNoteItemRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, debitNoteItem);

        }

        [AbpAuthorize(AppPermissions.Pages_DebitNoteItems_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _debitNoteItemRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_DebitNoteItems)]
        public async Task<List<DebitNoteItemMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new DebitNoteItemMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }

    }
}