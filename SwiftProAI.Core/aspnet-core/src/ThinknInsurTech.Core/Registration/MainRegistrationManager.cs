using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Threading.Tasks;

namespace ThinknInsurTech.Registration
{
    public class MainRegistrationManager : ThinknInsurTechDomainServiceBase, IMainRegistrationManager
    {
        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MainRegistrationManager(
            IRepository<MainRegistration, int> mainRegistrationRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _mainRegistrationRepository = mainRegistrationRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<int> CreateMainRegistrationAsync(MainRegistration registration)
        {
            int id = default; 
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                    id = await _mainRegistrationRepository.InsertAndGetIdAsync(registration);
                    await CurrentUnitOfWork.SaveChangesAsync();
            });
            return id;
        }

    }
}
