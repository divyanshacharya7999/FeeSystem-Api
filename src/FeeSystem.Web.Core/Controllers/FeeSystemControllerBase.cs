using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace FeeSystem.Controllers
{
    public abstract class FeeSystemControllerBase: AbpController
    {
        protected FeeSystemControllerBase()
        {
            LocalizationSourceName = FeeSystemConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
