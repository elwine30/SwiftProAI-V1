using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Remarks;

namespace ThinknInsurTech.Remarks
{
    public class RemarkManager : ThinknInsurTechDomainServiceBase, IRemarkManager
    {
        private readonly IRepository<Remark, int> _remarkRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RemarkManager(
            IRepository<Remark, int> remarkRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _remarkRepository = remarkRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<int> CreateRemarkAsync(Remark remark)
        {
            int id = default;
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                id = await _remarkRepository.InsertAndGetIdAsync(remark);
                await CurrentUnitOfWork.SaveChangesAsync();
            });
            return id;
        }

        public async Task<List<Remark>> GetAllRemarkAsync()
        {
            var details = await _remarkRepository.GetAll()
                                   .Where(m => m.Id > 0)
                                   .OrderByDescending(m => m.Id)
                                   .ToListAsync();

            return details;
        }

        public async Task<Remark> GetRemarkbyIdAsync(int remarkId)
        {
            var details = await _remarkRepository.GetAsync(remarkId);

            return details;
        }

        public async Task<List<Remark>> GetAllRemarksByRegistrationId(int registrationId)
        {
            var details = await _remarkRepository.GetAll()
                .Where(m => m.RegisterId.Equals(registrationId))
                .ToListAsync();

            return details;
        }
    }
}
