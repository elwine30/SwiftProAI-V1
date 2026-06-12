using Abp.AutoMapper;
using ThinknInsurTech.Authorization.Users.Dto;

namespace ThinknInsurTech.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
