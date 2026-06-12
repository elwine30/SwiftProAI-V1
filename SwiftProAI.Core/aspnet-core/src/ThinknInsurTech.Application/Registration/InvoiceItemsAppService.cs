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
    [AbpAuthorize(AppPermissions.Pages_InvoiceItems)]
    public class InvoiceItemsAppService : ThinknInsurTechAppServiceBase, IInvoiceItemsAppService
    {
        private readonly IRepository<InvoiceItem> _invoiceItemRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;

        public InvoiceItemsAppService(IRepository<InvoiceItem> invoiceItemRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository)
        {
            _invoiceItemRepository = invoiceItemRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;

        }

        public virtual async Task<PagedResultDto<GetInvoiceItemForViewDto>> GetAll(GetAllInvoiceItemsInput input)
        {

            var filteredInvoiceItems = _invoiceItemRepository.GetAll()
                        .Include(e => e.RegisterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id.ToString() == input.RegisterIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemTypeFilter), e => e.ItemType.Contains(input.ItemTypeFilter));

            var pagedAndFilteredInvoiceItems = filteredInvoiceItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var invoiceItems = from o in pagedAndFilteredInvoiceItems
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

            var totalCount = await filteredInvoiceItems.CountAsync();

            var dbList = await invoiceItems.ToListAsync();
            var results = new List<GetInvoiceItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetInvoiceItemForViewDto()
                {
                    InvoiceItem = new InvoiceItemDto
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

            return new PagedResultDto<GetInvoiceItemForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<List<InvoiceItemDto>> GetInvoiceItemByRegisterId(EntityDto input)
        {

            var filteredInvoiceItems = _invoiceItemRepository.GetAll()
                        .Where(w => w.RegisterId.Equals(input.Id));

            var invoiceItems = from o in filteredInvoiceItems
                               select new
                               {
                                   o.ItemType,
                                   o.Amount,
                                   o.Remark,
                                   o.Id,
                               };

            var dbList = await invoiceItems.ToListAsync();
            var results = new List<InvoiceItemDto>();

            foreach (var o in dbList)
            {
                var res = new InvoiceItemDto()
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

        public virtual async Task<List<InvoiceItemAmountDto>> GetInvoiceItemAmountsByRegisterId(EntityDto input)
        {

            var filteredInvoiceItems = _invoiceItemRepository.GetAll()
                        .Where(w => w.RegisterId.Equals(input.Id));

            var invoiceItems = from o in filteredInvoiceItems
                               select new
                               {
                                   o.ItemType,
                                   o.Amount,
                                   o.Id,
                               };

            var dbList = await invoiceItems.ToListAsync();
            var results = new List<InvoiceItemAmountDto>();

            foreach (var o in dbList)
            {
                var res = new InvoiceItemAmountDto()
                {

                    ItemType = o.ItemType,
                    Amount = o.Amount,
                    Id = o.Id,

                };

                results.Add(res);
            }

            return results;

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceItems_Edit)]
        public virtual async Task<GetInvoiceItemForEditOutput> GetInvoiceItemForEdit(EntityDto input)
        {
            var invoiceItem = _invoiceItemRepository.GetAll().Where(w => w.Id.Equals(input.Id)).FirstOrDefault();
            var output = new GetInvoiceItemForEditOutput { InvoiceItem = ObjectMapper.Map<CreateOrEditInvoiceItemDto>(invoiceItem) };


            var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.InvoiceItem.RegisterId);
            output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditInvoiceItemDto input)
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

        [AbpAuthorize(
         AppPermissions.Pages_InvoiceItems_Create,
         AppPermissions.Pages_InvoiceItems_Create_Admin,
         AppPermissions.Pages_InvoiceItems_Create_Adjuster
        )]
        protected virtual async Task Create(CreateOrEditInvoiceItemDto input)
        {
            var invoiceItem = ObjectMapper.Map<InvoiceItem>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                invoiceItem.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }


            if (AbpSession.TenantId != null)
            {
                invoiceItem.TenantId = (int)AbpSession.TenantId;
            }

            await _invoiceItemRepository.InsertAsync(invoiceItem);

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceItems_Edit)]
        protected virtual async Task Update(CreateOrEditInvoiceItemDto input)
        {
            var invoiceItem = await _invoiceItemRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, invoiceItem);

        }

        [AbpAuthorize(AppPermissions.Pages_InvoiceItems_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _invoiceItemRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_InvoiceItems)]
        public async Task<List<InvoiceItemMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new InvoiceItemMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }

    }
}