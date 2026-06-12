using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinknInsurTech.Case
{
    public class StatusManager : ThinknInsurTechDomainServiceBase, IStatusManager
    {
        private readonly IRepository<Status, int> _statusRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public StatusManager(
            IRepository<Status, int> statusRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _statusRepository = statusRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<int> CreateStatusAsync(Status casetype)
        {
            int id = default;
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                id = await _statusRepository.InsertAndGetIdAsync(casetype);
                await CurrentUnitOfWork.SaveChangesAsync();
            });
            return id;
        }

        public async Task<List<Status>> GetAllStatusAsync()
        {
            var details = await _statusRepository.GetAll()
                                   .Where(m => m.Id > 0)
                                   .OrderByDescending(m => m.Id)
                                   .ToListAsync();

            return details;
        }

        public async Task<Status> GetStatusbyIdAsync(int statusId)
        {
            var details = await _statusRepository.GetAsync(statusId);

            return details;
        }

    }
}