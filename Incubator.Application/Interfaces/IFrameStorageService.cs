using Incubator.Domain.Entities;

namespace Incubator.Application.Interfaces
{
    public interface IFrameStorageService
    {
        Task SaveFrameAsync(IncubatorFrame frame);
    }
}
