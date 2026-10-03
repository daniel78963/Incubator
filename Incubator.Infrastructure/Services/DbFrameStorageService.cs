using Incubator.Domain.Entities;
using Incubator.Infrastructure.Data;

namespace Incubator.Infrastructure.Services
{
    public class DbFrameStorageService
    {
        private readonly MyDbContext _dbContext;

        public DbFrameStorageService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveFrameAsync(IncubatorFrame frame)
        {
            _dbContext.IncubatorFrames.Add(frame);
            await _dbContext.SaveChangesAsync();
        }
    }
}
