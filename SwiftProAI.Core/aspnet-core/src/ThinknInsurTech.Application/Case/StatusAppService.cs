using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Case.Dto;

namespace ThinknInsurTech.Case
{
    [AbpAuthorize]
    public class StatusAppService : ThinknInsurTechAppServiceBase, IStatusAppService
    {
        private readonly IStatusManager _statusManager;
        private readonly IRepository<Status, int> _statusRepository;

        public StatusAppService(
            IStatusManager statusManager,
            IRepository<Status, int> statusRepository)

        {
            _statusManager = statusManager;
            _statusRepository = statusRepository;
        }


        public async Task<StatusDto> GetStatusDetailsbyId(int id)
        {
            var statusDetail = await _statusManager.GetStatusbyIdAsync(id);
            var _statusDto = new StatusDto
            {
                Id = statusDetail.Id,
                Code = statusDetail.Code,
                Description = statusDetail.Description,
                Closeflag = statusDetail.Closeflag,
                Type = statusDetail.Type
            };

            return _statusDto;
        }

        public async Task<ListResultDto<StatusDto>> GetAllStatusDetails()
        {
            var sourceStatusDetails = await _statusManager.GetAllStatusAsync();

            var statusList = new List<StatusDto>();
            foreach (var item in sourceStatusDetails)
            {
                statusList.Add(new StatusDto
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description,
                    Closeflag = item.Closeflag,
                    Type = item.Type
                });
            }

            return new ListResultDto<StatusDto>(statusList);
        }

        public async Task<int> CreateStatus(StatusDto input)
        {
            var sourceStatus = new Status
            {
                Code = input.Code,
                Description = input.Description,
                Closeflag = input.Closeflag,
                Type = input.Type
            };

            var id = await _statusManager.CreateStatusAsync(sourceStatus);

            return id;
        }

    }
}
