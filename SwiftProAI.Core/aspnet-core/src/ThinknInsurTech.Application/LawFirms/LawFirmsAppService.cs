using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.LawFirms.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Approval.Dtos;
using ThinknInsurTech.Approval;
using Abp.Domain.Uow;
using System;

namespace ThinknInsurTech.LawFirms
{
    [AbpAuthorize(AppPermissions.Pages_Administration_LawFirms)]
    public class LawFirmsAppService : ThinknInsurTechAppServiceBase, ILawFirmsAppService
    {
        private readonly IRepository<LawFirm> _lawFirmRepository;

        private readonly IRepository<ViewThirdPartyCaseRequest> _viewThirdPartyCaseRequestRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public LawFirmsAppService(IRepository<LawFirm> lawFirmRepository, IRepository<ViewThirdPartyCaseRequest> viewThirdPartyCaseRequestRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _lawFirmRepository = lawFirmRepository;
            _viewThirdPartyCaseRequestRepository = viewThirdPartyCaseRequestRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task<PagedResultDto<GetLawFirmForViewDto>> GetAll(GetAllLawFirmsInput input)
        {

            var filteredLawFirms = _lawFirmRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ShortName.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortNameFilter), e => e.ShortName.Contains(input.ShortNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredLawFirms = filteredLawFirms
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var lawFirms = from o in pagedAndFilteredLawFirms
                           select new
                           {

                               o.Name,
                               o.ShortName,
                               o.Address,
                               Id = o.Id,
                               o.IsActive,
                               o.BusinessRegistrationNo,
                               o.AssignOUId,
                               o.AllowToViewAssignedCases,
                               o.ViewThirdPartyCaseRequestFk,
                               o.ViewThirdPartyCaseRequestId
                           };

            var totalCount = await filteredLawFirms.CountAsync();

            var dbList = await lawFirms.ToListAsync();
            var results = new List<GetLawFirmForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLawFirmForViewDto()
                {
                    LawFirm = new LawFirmDto
                    {
                        Name = o.Name,
                        ShortName = o.ShortName,
                        Address = o.Address,
                        Id = o.Id,
                        IsActive = o.IsActive,
                        BusinessRegistrationNo = o.BusinessRegistrationNo,
                        AssignOUId = o.AssignOUId,
                        AllowToViewAssignedCases = o.AllowToViewAssignedCases,
                        ViewThirdPartyCaseRequestId = o.ViewThirdPartyCaseRequestId,
                        ViewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                        {
                            Status = o.ViewThirdPartyCaseRequestFk != null ? o.ViewThirdPartyCaseRequestFk.Status : null,
                            CancelRemark = o.ViewThirdPartyCaseRequestFk != null ? o.ViewThirdPartyCaseRequestFk.CancelRemark : null,
                        }
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLawFirmForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetLawFirmForViewDto> GetLawFirmForView(int id)
        {
            var lawFirm = await _lawFirmRepository.GetAsync(id);

            var output = new GetLawFirmForViewDto { LawFirm = ObjectMapper.Map<LawFirmDto>(lawFirm) };

            if (output.LawFirm.ViewThirdPartyCaseRequestId != null)
            {
                var _viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)output.LawFirm.ViewThirdPartyCaseRequestId);
                output.LawFirm.ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(_viewThirdPartyCaseRequest);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_LawFirms_Edit)]
        public virtual async Task<GetLawFirmForEditOutput> GetLawFirmForEdit(EntityDto input)
        {
            var lawFirm = await _lawFirmRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLawFirmForEditOutput { LawFirm = ObjectMapper.Map<CreateOrEditLawFirmDto>(lawFirm) };

            if (output.LawFirm.ViewThirdPartyCaseRequestId != null)
            {
                var _viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)output.LawFirm.ViewThirdPartyCaseRequestId);
                output.LawFirm.ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(_viewThirdPartyCaseRequest);
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditLawFirmDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_LawFirms_Create)]
        protected virtual async Task Create(CreateOrEditLawFirmDto input)
        {
            var lawFirm = ObjectMapper.Map<LawFirm>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                lawFirm.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;
            }

            if (input.AllowToViewAssignedCases)
            {
                var viewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                {
                    AssignByOU = AbpSession.GetCurrentOUId() ?? null,
                    BusinessRegistrationNo = input.BusinessRegistrationNo,
                    CompanyName = input.Name,
                };

                lawFirm.ViewThirdPartyCaseRequestFk = ObjectMapper.Map<ViewThirdPartyCaseRequest>(viewThirdPartyCaseRequest);
                lawFirm.ViewThirdPartyCaseRequestFk.TenantId = (int?)AbpSession.TenantId;
                lawFirm.ViewThirdPartyCaseRequestFk.Status = "Pending Approval";
            }

            if (AbpSession.TenantId != null)
            {
                lawFirm.TenantId = (int?)AbpSession.TenantId;
            }

            await _lawFirmRepository.InsertAsync(lawFirm);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_LawFirms_Edit)]
        protected virtual async Task Update(CreateOrEditLawFirmDto input)
        {
            var lawFirm = await _lawFirmRepository.FirstOrDefaultAsync((int)input.Id);

            if (input.ViewThirdPartyCaseRequestId != null)
            {
                await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    var entity = await _viewThirdPartyCaseRequestRepository.GetAll()
                    .Where(x => x.Id == (int)input.ViewThirdPartyCaseRequestId)
                    .FirstOrDefaultAsync();

                if (entity != null)
                {
                    entity.CancelRemark = input.ViewThirdPartyCaseRequest.CancelRemark;
                    entity.CancelledBy = (int)AbpSession.UserId.Value;
                    entity.CancelledDate = DateTime.Now;
                    entity.CompanyName = input.Name;

                        // allow adjuster to update if request is pendingApproval or Cancelled
                        if (entity.Status != "Approved")
                        {
                            entity.BusinessRegistrationNo = input.BusinessRegistrationNo;
                        }

                        await _viewThirdPartyCaseRequestRepository.UpdateAsync(entity);
                    }
                });
            }
            else
            {
                if (input.AllowToViewAssignedCases)
                {
                    var viewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                    {
                        AssignByOU = AbpSession.GetCurrentOUId() ?? null,
                        BusinessRegistrationNo = input.BusinessRegistrationNo
                    };

                    lawFirm.ViewThirdPartyCaseRequestFk = ObjectMapper.Map<ViewThirdPartyCaseRequest>(viewThirdPartyCaseRequest);

                    lawFirm.ViewThirdPartyCaseRequestFk.TenantId = (int)AbpSession.TenantId;
                    lawFirm.ViewThirdPartyCaseRequestFk.Status = "Pending Approval";
                }
            }
            ObjectMapper.Map(input, lawFirm);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_LawFirms_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _lawFirmRepository.DeleteAsync(input.Id);
        }


    }
}