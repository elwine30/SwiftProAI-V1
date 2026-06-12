using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.EntityFrameworkCore;
using ThinknInsurTech.Notifications;

namespace ThinknInsurTech.Migrations.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly ThinknInsurTechDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(ThinknInsurTechDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //User role

            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true });
                _context.SaveChanges();
            }

            //Super Admin role

            var superAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.SuperAdmin);
            if (superAdminRole == null)
            {
                superAdminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.SuperAdmin, StaticRoleNames.Tenants.SuperAdmin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = false;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }

            var superAdminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == StaticRoleNames.Tenants.SuperAdminUsername);
            if (superAdminUser == null)
            {
                superAdminUser = User.CreateSuperAdminUser(_tenantId, StaticRoleNames.Tenants.SuperAdminUsername, "superadmin@extendedDefaultTenant.com");
                superAdminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(superAdminUser, "123qwe");
                superAdminUser.IsEmailConfirmed = true;
                superAdminUser.ShouldChangePasswordOnNextLogin = false;
                superAdminUser.IsActive = true;

                _context.Users.Add(superAdminUser);
                _context.SaveChanges();

                //Assign Super Admin role to super admin user
                _context.UserRoles.Add(new UserRole(_tenantId, superAdminUser.Id, superAdminRole.Id));
                _context.SaveChanges();

                //User account of super admin user
                var isSuperAdminAccExist = _context.UserAccounts.Where(x => x.UserName == StaticRoleNames.Tenants.SuperAdminUsername).FirstOrDefault();
                if (_tenantId == 1 && isSuperAdminAccExist == null)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = superAdminUser.Id,
                        UserName = StaticRoleNames.Tenants.SuperAdminUsername,
                        EmailAddress = superAdminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, superAdminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }
        }
    }
}
