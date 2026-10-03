using Incubator.Domain.Entities;

namespace Incubator.Application.Interfaces
{
    public interface IAppConfigService
    {
        AppConfiguration GetConfig();
        void SaveConfig(AppConfiguration config);
    }
}
