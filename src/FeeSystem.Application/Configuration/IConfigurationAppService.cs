using System.Threading.Tasks;
using FeeSystem.Configuration.Dto;

namespace FeeSystem.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
