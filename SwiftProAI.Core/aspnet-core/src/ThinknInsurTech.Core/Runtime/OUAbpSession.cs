using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.Runtime;
using Abp.Runtime.Session;
using System.Linq;
using ThinknInsurTech.Authorization.Users;

namespace ThinknInsurTech.Runtime
{
    public class OUAbpSession : ClaimsAbpSession, IOUAbpSession, ISingletonDependency
    {
        private readonly IRepository<User, long> _userRepository;

        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnit;

        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        private long? _currentOUId;

        public OUAbpSession(IRepository<User, long> userRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnit,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) :
            base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
            _userRepository = userRepository;
            _userOrganizationUnit = userOrganizationUnit;
            _organizationUnitRepository = organizationUnitRepository;
        }

        public long? CurrentOUId
        {
            get
            {
                if (_currentOUId.HasValue)
                {
                    return _currentOUId;
                }

                if (UserId != null)
                {
                    try
                    {
                        var user = _userRepository.FirstOrDefault(UserId.Value);

                        if(user != null)
                        {

                            // need to find a way to filter super admin user by rolename, to bypass getting super admin's OUId which is always null

                            var ouId = _userOrganizationUnit.GetAll()
                            .Where(ou => !ou.IsDeleted && ou.UserId == user.Id)
                            .Join(_organizationUnitRepository.GetAll(), uOU => uOU.OrganizationUnitId, ou => ou.Id, (uOU, ou) => new { uOU, ou })
                            .Select(joined => (int?)joined.ou.Id)
                            .FirstOrDefault();

                            _currentOUId = ouId;
                            return _currentOUId;
                        } 
                        else
                        {
                            return null;
                        }

                    } catch(EntityNotFoundException ex)
                    {
                        return null;
                    }
                }

                return null;
            }
        }
    }
}
