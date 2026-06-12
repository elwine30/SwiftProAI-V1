using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Tenants
{
    public class AddStaticRoles
    {
        private readonly ThinknInsurTechDbContext _context;
        private readonly int _tenantId;

        public AddStaticRoles(ThinknInsurTechDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Add()
        {
            // Add Lawyer, Insurer, Workshop, Adjuster, Editor and Finance role
            EnsureRoleExists(StaticRoleNames.Tenants.Lawyer);
            EnsureRoleExists(StaticRoleNames.Tenants.Insurer);
            EnsureRoleExists(StaticRoleNames.Tenants.Workshop);
            EnsureRoleExists(StaticRoleNames.Tenants.Adjuster);
            EnsureRoleExists(StaticRoleNames.Tenants.Editor);
            EnsureRoleExists(StaticRoleNames.Tenants.Finance);
            EnsureRoleExists(StaticRoleNames.Tenants.SuperAdmin);
        }

        private void EnsureRoleExists(string roleName)
        {
            var role = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == roleName);
            if (role == null)
            {
                role = _context.Roles.Add(new Role(_tenantId, roleName, roleName) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
        }
    }
}
