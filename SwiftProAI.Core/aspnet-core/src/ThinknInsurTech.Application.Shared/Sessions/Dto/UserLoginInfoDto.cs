using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using System.Collections.Generic;

namespace ThinknInsurTech.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }

        public long? OrganizationUnitId { get; set; }
    }
}
