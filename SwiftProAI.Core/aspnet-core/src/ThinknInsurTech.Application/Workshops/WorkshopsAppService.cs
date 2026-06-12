using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Workshops.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Approval.Dtos;
using ThinknInsurTech.Approval;
using Abp.Domain.Uow;

namespace ThinknInsurTech.Workshops
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Workshops)]
    public class WorkshopsAppService : ThinknInsurTechAppServiceBase, IWorkshopsAppService
    {
        private readonly IRepository<Workshop> _workshopRepository;

        private readonly IRepository<ViewThirdPartyCaseRequest> _viewThirdPartyCaseRequestRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public WorkshopsAppService(IRepository<Workshop> workshopRepository, IRepository<ViewThirdPartyCaseRequest> viewThirdPartyCaseRequestRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _workshopRepository = workshopRepository;
            _viewThirdPartyCaseRequestRepository = viewThirdPartyCaseRequestRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task<PagedResultDto<GetWorkshopForViewDto>> GetAll(GetAllWorkshopsInput input)
        {

            var filteredWorkshops = _workshopRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WorkshopNo.Contains(input.Filter) || e.WorkshopName.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkshopNoFilter), e => e.WorkshopNo.Contains(input.WorkshopNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkshopNameFilter), e => e.WorkshopName.Contains(input.WorkshopNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(input.MinClaimRateFilter != null, e => e.ClaimRate >= input.MinClaimRateFilter)
                        .WhereIf(input.MaxClaimRateFilter != null, e => e.ClaimRate <= input.MaxClaimRateFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredWorkshops = filteredWorkshops
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var workshops = from o in pagedAndFilteredWorkshops
                            select new
                            {

                                o.WorkshopNo,
                                o.WorkshopName,
                                o.Address,
                                o.ClaimRate,
                                o.IsActive,
                                Id = o.Id,
                                o.BusinessRegistrationNo,
                                o.AssignOUId,
                                o.AllowToViewAssignedCases,
                                o.ViewThirdPartyCaseRequestFk,
                                o.ViewThirdPartyCaseRequestId
                            };

            var totalCount = await filteredWorkshops.CountAsync();

            var dbList = await workshops.ToListAsync();
            var results = new List<GetWorkshopForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWorkshopForViewDto()
                {
                    Workshop = new WorkshopDto
                    {
                        WorkshopNo = o.WorkshopNo,
                        WorkshopName = o.WorkshopName,
                        Address = o.Address,
                        ClaimRate = o.ClaimRate,
                        IsActive = o.IsActive,
                        Id = o.Id,
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

            return new PagedResultDto<GetWorkshopForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetWorkshopForViewDto> GetWorkshopForView(int id)
        {
            var workshop = await _workshopRepository.GetAsync(id);

            var output = new GetWorkshopForViewDto { Workshop = ObjectMapper.Map<WorkshopDto>(workshop) };

            if (output.Workshop.ViewThirdPartyCaseRequestId != null)
            {
                var _viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)output.Workshop.ViewThirdPartyCaseRequestId);
                output.Workshop.ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(_viewThirdPartyCaseRequest);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Workshops_Edit)]
        public virtual async Task<GetWorkshopForEditOutput> GetWorkshopForEdit(EntityDto input)
        {
            var workshop = await _workshopRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWorkshopForEditOutput { Workshop = ObjectMapper.Map<CreateOrEditWorkshopDto>(workshop) };

            if (output.Workshop.ViewThirdPartyCaseRequestId != null)
            {
                var _viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)output.Workshop.ViewThirdPartyCaseRequestId);
                output.Workshop.ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(_viewThirdPartyCaseRequest);
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditWorkshopDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Workshops_Create)]
        protected virtual async Task Create(CreateOrEditWorkshopDto input)
        {
            var workshop = ObjectMapper.Map<Workshop>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                workshop.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;
            }

            if (input.AllowToViewAssignedCases)
            {
                var viewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                {
                    AssignByOU = AbpSession.GetCurrentOUId() ?? null,
                    BusinessRegistrationNo = input.BusinessRegistrationNo,
                    CompanyName = input.WorkshopName,
                };

                workshop.ViewThirdPartyCaseRequestFk = ObjectMapper.Map<ViewThirdPartyCaseRequest>(viewThirdPartyCaseRequest);

                workshop.ViewThirdPartyCaseRequestFk.TenantId = (int?)AbpSession.TenantId;
                workshop.ViewThirdPartyCaseRequestFk.Status = "Pending Approval";
            }

            if (AbpSession.TenantId != null)
            {
                workshop.TenantId = (int?)AbpSession.TenantId;
            }

            await _workshopRepository.InsertAsync(workshop);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Workshops_Edit)]
        protected virtual async Task Update(CreateOrEditWorkshopDto input)
        {
            var workshop = await _workshopRepository.FirstOrDefaultAsync((int)input.Id);

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
                    entity.CompanyName = input.WorkshopName;

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

                    workshop.ViewThirdPartyCaseRequestFk = ObjectMapper.Map<ViewThirdPartyCaseRequest>(viewThirdPartyCaseRequest);

                    workshop.ViewThirdPartyCaseRequestFk.TenantId = (int)AbpSession.TenantId;
                    workshop.ViewThirdPartyCaseRequestFk.Status = "Pending Approval";
                }
            }

            ObjectMapper.Map(input, workshop);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Workshops_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _workshopRepository.DeleteAsync(input.Id);
        }

    }
}