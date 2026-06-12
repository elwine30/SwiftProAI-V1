using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.MultiTenancy;

namespace ThinknInsurTech.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}