using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Companies;

namespace ThinknInsurTech.Organizations
{
    public class ViewThirdPartyCasesManager : ThinknInsurTechDomainServiceBase, IViewThirdPartyCasesManager
    {
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ViewThirdPartyCasesManager(IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task CreateMainRegistrationOU(ViewThirdPartyCases input)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await _mainRegistrationOrganizationUnitRepository.InsertAsync(input);
            });
        }

        public async Task<ViewThirdPartyCases> GetMainRegistrationOUByAssignedOUIdAsync(long assignedOUId, int registerId)
        {
            var details = await _mainRegistrationOrganizationUnitRepository.GetAll()
                .Where(x => x.AssignedOUId == assignedOUId && x.RegisterId == registerId)
                .FirstOrDefaultAsync();

            return details;
        }

        public async Task UpdateMainRegistrationOU(long assignedOUId, int registerId, long newAssignedOUId)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var detail = await _mainRegistrationOrganizationUnitRepository.GetAll()
                .Where(x => x.AssignedOUId == assignedOUId && x.RegisterId == registerId)
                .FirstOrDefaultAsync();

                if(detail != null)
                {
                    detail.AssignedOUId = newAssignedOUId;

                    await _mainRegistrationOrganizationUnitRepository.UpdateAsync(detail);
                }
            });
        }

        public async Task DeleteMainRegistrationOU(long? ouId, int registerId)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var entityList = await _mainRegistrationOrganizationUnitRepository.GetAll().AsNoTracking()
                .Where(x => x.AssignedOUId == ouId && x.RegisterId == registerId)
                .ToListAsync();

                foreach (var entity in entityList)
                {
                    await _mainRegistrationOrganizationUnitRepository.DeleteAsync(entity);
                }
            });
        }
    }
}
