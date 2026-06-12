using Abp.AspNetCore.Mvc.Authorization;
using ThinknInsurTech.Authorization.Users.Profile;
using ThinknInsurTech.Graphics;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}