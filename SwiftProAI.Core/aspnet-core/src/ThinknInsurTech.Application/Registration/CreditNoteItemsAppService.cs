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
    [AbpAuthorize(AppPermissions.Pages_CreditNoteItems)]
    public class CreditNoteItemsAppService : ThinknInsurTechAppServiceBase, ICreditNoteItemsAppService
    {
        private readonly IRepository<CreditNoteItem> _creditNoteItemRepository;
        private readonly IRepository<InvoiceItem> _invoiceItemRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;

        public CreditNoteItemsAppService(IRepository<CreditNoteItem> creditNoteItemRepository,
            IRepository<InvoiceItem> invoiceItemRepository,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository)
        {
            _creditNoteItemRepository = creditNoteItemRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
        }

        public virtual async Task<PagedResultDto<GetCreditNoteItemForViewDto>> GetAll(GetAllCreditNoteItemsInput input)
        {

            var filteredCreditNoteItems = _creditNoteItemRepository.GetAll()
                        .Include(e => e.RegisterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id.ToString() == input.RegisterIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemTypeFilter), e => e.ItemType.Contains(input.ItemTypeFilter));

            var pagedAndFilteredCreditNoteItems = filteredCreditNoteItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditNoteItems = from o in pagedAndFilteredCreditNoteItems
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

            var totalCount = await filteredCreditNoteItems.CountAsync();

            var dbList = await creditNoteItems.ToListAsync();
            var results = new List<GetCreditNoteItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCreditNoteItemForViewDto()
                {
                    CreditNoteItem = new CreditNoteItemDto
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

            return new PagedResultDto<GetCreditNoteItemForViewDto>(
                totalCount,
                results
            );
        }

        public virtual async Task<List<CreditNoteItemAmountDto>> GetCreditNoteItemAmountsByRegisterId(EntityDto input)
        {

            var filteredCreditNoteItems = _creditNoteItemRepository.GetAll()
                        .Where(w => w.RegisterId.Equals(input.Id));

            var creditNoteItems = from o in filteredCreditNoteItems
                                  select new
                                  {
                                      o.ItemType,
                                      o.Amount,
                                      o.Id,
                                  };

            var dbList = await creditNoteItems.ToListAsync();
            var results = new List<CreditNoteItemAmountDto>();

            foreach (var o in dbList)
            {
                var res = new CreditNoteItemAmountDto()
                {

                    ItemType = o.ItemType,
                    Amount = o.Amount,
                    Id = o.Id,

                };

                results.Add(res);
            }

            return results;

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteItems_Edit)]
        public virtual async Task<GetCreditNoteItemForEditOutput> GetCreditNoteItemForEdit(EntityDto input)
        {
            var creditNoteItem = await _creditNoteItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCreditNoteItemForEditOutput { CreditNoteItem = ObjectMapper.Map<CreateOrEditCreditNoteItemDto>(creditNoteItem) };

            if (output.CreditNoteItem != null)
            {
                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CreditNoteItem.RegisterId);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            }

            return output;
        }

        public virtual async Task BulkCreate(int registerId)
        {
            //if (!_creditNoteItemRepository.GetAll().Any(a => a.RegisterId.Equals(registerId)))
            //{
            var main = await _lookup_mainRegistrationRepository.GetAll()
                            .Include(i => i.CreditNoteItems)
                            .Include(i => i.InvoiceItems)
                            .FirstOrDefaultAsync(w => w.Id.Equals(registerId));
            //var invoiceItems = await _invoiceItemRepository.GetAll().Where(w => w.RegisterId.Equals(registerId)).ToListAsync();
            if (main.InvoiceItems.Count > 0 && !main.CreditNoteItems.Any())
            {
                main.CreditNoteItems = [];
                foreach (var input in main.InvoiceItems)
                {
                    main.CreditNoteItems.Add(new CreditNoteItem
                    {
                        ItemType = input.ItemType,
                        Amount = input.Amount,
                        Remark = input.Remark,
                        TenantId = (int)AbpSession.TenantId
                    });
                }
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            //}
        }

        public virtual async Task CreateOrEdit(CreateOrEditCreditNoteItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CreditNoteItems_Create, AppPermissions.Pages_CreditNoteItems_Create_Adjuster, AppPermissions.Pages_CreditNoteItems_Create_Admin)]
        protected virtual async Task Create(CreateOrEditCreditNoteItemDto input)
        {
            var creditNoteItem = ObjectMapper.Map<CreditNoteItem>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                creditNoteItem.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                creditNoteItem.TenantId = (int)AbpSession.TenantId;
            }

            await _creditNoteItemRepository.InsertAsync(creditNoteItem);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteItems_Edit)]
        protected virtual async Task Update(CreateOrEditCreditNoteItemDto input)
        {
            var creditNoteItem = await _creditNoteItemRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, creditNoteItem);

        }

        [AbpAuthorize(AppPermissions.Pages_CreditNoteItems_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _creditNoteItemRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_CreditNoteItems)]
        public async Task<List<CreditNoteItemMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
        {
            return await _lookup_mainRegistrationRepository.GetAll()
                .Select(mainRegistration => new CreditNoteItemMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration == null || mainRegistration.VehicleNo == null ? "" : mainRegistration.VehicleNo.ToString()
                }).ToListAsync();
        }

    }
}