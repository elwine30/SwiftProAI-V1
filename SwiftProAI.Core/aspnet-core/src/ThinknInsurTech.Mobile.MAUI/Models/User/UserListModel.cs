using Abp.AutoMapper;
using ThinknInsurTech.Authorization.Users.Dto;

namespace ThinknInsurTech.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(UserListDto))]
    public class UserListModel : UserListDto
    {
        public string Photo { get; set; }

        public string FullName => Name + " " + Surname;
    }
}
