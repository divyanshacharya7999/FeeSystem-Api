using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using FeeSystem.Configuration.Dto;

namespace FeeSystem.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : FeeSystemAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
