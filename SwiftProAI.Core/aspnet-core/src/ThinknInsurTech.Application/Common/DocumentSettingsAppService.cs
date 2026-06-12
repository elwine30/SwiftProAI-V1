using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_Administration_DocumentSettings)]
    public class DocumentSettingsAppService : ThinknInsurTechAppServiceBase, IDocumentSettingsAppService
    {
        private readonly IRepository<DocumentSetting> _documentSettingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DocumentSettingsAppService(IRepository<DocumentSetting> documentSettingRepository, IUnitOfWorkManager unitOfWorkManager
)
        {
            _documentSettingRepository = documentSettingRepository;
            _unitOfWorkManager = unitOfWorkManager;


        }

        public async Task<PagedResultDto<GetDocumentSettingForViewDto>> GetAll(GetAllDocumentSettingsInput input)
        {

            var filteredDocumentSettings = _documentSettingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.businessRegistrationNo.Contains(input.Filter) || e.companyLegalName.Contains(input.Filter) || e.address.Contains(input.Filter) || e.taxNo.Contains(input.Filter) || e.telNo.Contains(input.Filter) || e.invoiceRefNoPrefix.Contains(input.Filter) || e.debitRefNoPrefix.Contains(input.Filter) || e.creditRefNoPrefix.Contains(input.Filter) || e.caseRefNoPrefix.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessRegistrationNoFilter), e => e.businessRegistrationNo.Contains(input.BusinessRegistrationNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.companyLegalNameFilter), e => e.companyLegalName.Contains(input.companyLegalNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.addressFilter), e => e.address.Contains(input.addressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.taxNoFilter), e => e.taxNo.Contains(input.taxNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.telNoFilter), e => e.telNo.Contains(input.telNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.invoiceRefNoPrefixFilter), e => e.invoiceRefNoPrefix.Contains(input.invoiceRefNoPrefixFilter))
                        .WhereIf(input.MininvoiceRefNoLengthFilter != null, e => e.invoiceRefNoLength >= input.MininvoiceRefNoLengthFilter)
                        .WhereIf(input.MaxinvoiceRefNoLengthFilter != null, e => e.invoiceRefNoLength <= input.MaxinvoiceRefNoLengthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.debitRefNoPrefixFilter), e => e.debitRefNoPrefix.Contains(input.debitRefNoPrefixFilter))
                        .WhereIf(input.MindebitRefNoLengthFilter != null, e => e.debitRefNoLength >= input.MindebitRefNoLengthFilter)
                        .WhereIf(input.MaxdebitRefNoLengthFilter != null, e => e.debitRefNoLength <= input.MaxdebitRefNoLengthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.creditRefNoPrefixFilter), e => e.creditRefNoPrefix.Contains(input.creditRefNoPrefixFilter))
                        .WhereIf(input.MincreditRefNoLengthFilter != null, e => e.creditRefNoLength >= input.MincreditRefNoLengthFilter)
                        .WhereIf(input.MaxcreditRefNoLengthFilter != null, e => e.creditRefNoLength <= input.MaxcreditRefNoLengthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.caseRefNoPrefixFilter), e => e.caseRefNoPrefix.Contains(input.caseRefNoPrefixFilter))
                        .WhereIf(input.MincaseRefNoLengthFilter != null, e => e.caseRefNoLength >= input.MincaseRefNoLengthFilter)
                        .WhereIf(input.MaxcaseRefNoLengthFilter != null, e => e.caseRefNoLength <= input.MaxcaseRefNoLengthFilter);

            var pagedAndFilteredDocumentSettings = filteredDocumentSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documentSettings = from o in pagedAndFilteredDocumentSettings
                                   select new
                                   {

                                       o.businessRegistrationNo,
                                       o.companyLegalName,
                                       o.address,
                                       o.taxNo,
                                       o.telNo,
                                       o.invoiceRefNoPrefix,
                                       o.invoiceRefNoLength,
                                       o.debitRefNoPrefix,
                                       o.debitRefNoLength,
                                       o.creditRefNoPrefix,
                                       o.creditRefNoLength,
                                       o.caseRefNoPrefix,
                                       o.caseRefNoLength,
                                       Id = o.Id,
                                       o.companyType,
                                   };

            var totalCount = await filteredDocumentSettings.CountAsync();

            var dbList = await documentSettings.ToListAsync();
            var results = new List<GetDocumentSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDocumentSettingForViewDto()
                {
                    DocumentSetting = new DocumentSettingDto
                    {

                        businessRegistrationNo = o.businessRegistrationNo,
                        companyLegalName = o.companyLegalName,
                        address = o.address,
                        taxNo = o.taxNo,
                        telNo = o.telNo,
                        invoiceRefNoPrefix = o.invoiceRefNoPrefix,
                        invoiceRefNoLength = o.invoiceRefNoLength,
                        debitRefNoPrefix = o.debitRefNoPrefix,
                        debitRefNoLength = o.debitRefNoLength,
                        creditRefNoPrefix = o.creditRefNoPrefix,
                        creditRefNoLength = o.creditRefNoLength,
                        caseRefNoPrefix = o.caseRefNoPrefix,
                        caseRefNoLength = o.caseRefNoLength,
                        Id = o.Id,
                        companyType = o.companyType
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDocumentSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDocumentSettingForViewDto> GetDocumentSettingForView(int id)
        {
            var documentSetting = await _documentSettingRepository.GetAsync(id);

            var output = new GetDocumentSettingForViewDto { DocumentSetting = ObjectMapper.Map<DocumentSettingDto>(documentSetting) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DocumentSettings_Edit)]
        public async Task<GetDocumentSettingForEditOutput> GetDocumentSettingForEdit()
        {
            var user = await UserManager.GetUserByIdAsync((int)AbpSession.UserId);
            var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);

            if (organizationUnits.Any())
            {
                var organizationUnitId = organizationUnits.Select(ou => ou.Id).FirstOrDefault();
                // Note: This code will look at the id of the first organization unit which the user belongs to.
                // If there is a possibility that one user can have multiple O.U's, then this code needs to be refactored.

                var documentSetting = await _documentSettingRepository.FirstOrDefaultAsync(p => p.OrganizationUnitId == organizationUnitId);
                var output = new GetDocumentSettingForEditOutput { DocumentSetting = ObjectMapper.Map<CreateOrEditDocumentSettingDto>(documentSetting) };

                return output;
            }
            else
            {
                throw new UserFriendlyException("The current logged in user does not belong to any Organization Units. Please add this user into a Organization Unit to access the Document Settings.");
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DocumentSettings_Create)]
        public async Task CreateOrEdit(CreateOrEditDocumentSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_DocumentSettings_Create)]
        protected async Task Create(CreateOrEditDocumentSettingDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if ((_documentSettingRepository.FirstOrDefault(f => f.businessRegistrationNo.ToLower() == input.businessRegistrationNo.ToLower())) != null)
                {
                    throw new UserFriendlyException("Company have registered");
                }

                if ((_documentSettingRepository.FirstOrDefault(f => f.caseRefNoPrefix.ToLower() == input.caseRefNoPrefix.ToLower())) != null)
                {
                    throw new UserFriendlyException($"{input.caseRefNoPrefix} is taken.");
                }

                var documentSetting = ObjectMapper.Map<DocumentSetting>(input);

                if (AbpSession.TenantId != null)
                {
                    documentSetting.TenantId = (int)AbpSession.TenantId;
                }

                if (input.organizationUnitId != 0)
                {
                    await _documentSettingRepository.InsertAsync(documentSetting);
                    return;
                }
                else
                {
                    var user = await UserManager.GetUserByIdAsync((int)AbpSession.UserId);
                    var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                    if (organizationUnits.Any())
                    {
                        var organizationUnitId = organizationUnits.Select(ou => ou.Id).FirstOrDefault();
                        documentSetting.OrganizationUnitId = organizationUnitId;
                        // Note: This code will look at the id of the first organization unit which the user belongs to.
                        // If there is a possibility that one user can have multiple O.U's, then this code needs to be refactored.
                    }
                    else
                    {
                        throw new UserFriendlyException("The current logged in user does not belong to any Organization Units. Please add this user into a Organization Unit to access the Document Settings.");
                    }

                    await _documentSettingRepository.InsertAsync(documentSetting);
                }



            }


        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DocumentSettings_Edit)]
        protected async Task Update(CreateOrEditDocumentSettingDto input)
        {
            var documentSetting = await _documentSettingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, documentSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DocumentSettings_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _documentSettingRepository.DeleteAsync(input.Id);
        }

    }
}