using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.OpenIddict.Applications;
using ThinknInsurTech.OpenIddict.Authorizations;
using ThinknInsurTech.OpenIddict.Scopes;
using ThinknInsurTech.OpenIddict.Tokens;

namespace ThinknInsurTech.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}