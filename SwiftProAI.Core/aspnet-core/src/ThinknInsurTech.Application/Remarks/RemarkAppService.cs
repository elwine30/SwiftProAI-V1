using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Remarks.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace ThinknInsurTech.Remarks
{
    [AbpAuthorize]
    public class RemarkAppService : ThinknInsurTechAppServiceBase, IRemarkAppService
    {
        private readonly IRemarkManager _remarkManager;
        private readonly IRepository<Remark, int> _remarkRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public RemarkAppService(
            IRemarkManager remarkManager,
            IRepository<Remark, int> remarkRepository,
            IRepository<User, long> userRepository)

        {
            _remarkManager = remarkManager;
            _remarkRepository = remarkRepository;
            _lookup_userRepository = userRepository;
        }


        public async Task<RemarkDto> GetRemarkDetailsbyId(int id)
        {
            var remarkDetail = await _remarkManager.GetRemarkbyIdAsync(id);
            var _remarkDto = new RemarkDto
            {
                Id = remarkDetail.Id,
                RegisterId = remarkDetail.RegisterId,
                Description = remarkDetail.Description,
            };

            return _remarkDto;
        }

        public async Task<ListResultDto<RemarkDto>> GetAllRemarkDetails()
        {
            var sourceRemarkDetails = await _remarkManager.GetAllRemarkAsync();

            var remarkList = new List<RemarkDto>();
            foreach (var item in sourceRemarkDetails)
            {
                remarkList.Add(new RemarkDto
                {
                    Id = item.Id,
                    RegisterId = item.RegisterId,
                    Description = item.Description,
                });
            }

            return new ListResultDto<RemarkDto>(remarkList);
        }

        public async Task<int> CreateRemark(RemarkDto input)
        {
            var currentUserIdentifier = AbpSession.ToUserIdentifier();
            var sourceRemark = new Remark
            {
                RegisterId = input.RegisterId,
                Description = input.Description,
                CreatorUserId = currentUserIdentifier.UserId,
                CreationTime = DateTime.Now,
            };

            var id = await _remarkManager.CreateRemarkAsync(sourceRemark);

            return id;
        }
        public async Task<PagedResultDto<RemarkDto>> GetAllRemarkByRegistrationId(RemarkInputDto remarkInput)
        {
            IQueryable<RemarkDto> sourceRemarkDetails = _remarkRepository.GetAll()
                .Where(x => x.RegisterId.Equals(remarkInput.RegisterId))
                .Join(_lookup_userRepository.GetAll(),
                x=>x.CreatorUserId,
                u=>u.Id,
                (x,u) => new {x,u})
                .Select(rm => new RemarkDto()
                {
                    RegisterId = rm.x.RegisterId,
                    Description = rm.x.Description,
                    CreationTime = rm.x.CreationTime,
                    CreatorUserName = rm.u.UserName,
                });
            remarkInput.MaxResultCount = 5;
            var remarkCount = await sourceRemarkDetails.CountAsync();
            var remarksDetails = await sourceRemarkDetails.OrderByDescending(x=>x.CreationTime).PageBy(remarkInput).ToListAsync();
            
            return new PagedResultDto<RemarkDto>(remarkCount, remarksDetails);

            //var sourceRemarkDetails = _remarkManager.GetAllRemarksByRegistrationId(remarkInput.RegisterId);
            //var remarkList = new List<RemarkDto>();
            //foreach (var item in sourceRemarkDetails)
            //{
            //    remarkList.Add(new RemarkDto
            //    {
            //        Id = item.Id,
            //        RegisterId = item.RegisterId,
            //        Description = item.Description,
            //        CreatorUserId = (int)item.CreatorUserId,
            //        CreationTime = item.CreationTime,
            //    });
            //}
            //var remarkCount = await sourceRemarkDetails.CountAsync();
        }

    }
}
