using ThinknInsurTech.Models.NavigationMenu;

namespace ThinknInsurTech.Services.Navigation
{
    public interface IMenuProvider
    {
        List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}