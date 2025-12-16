using Microsoft.EntityFrameworkCore;
using Skype.Data;

namespace Skype.Repositories
{
    public class RepositorySettings
    {
        AppDbContext _dbContext;

        public RepositorySettings(AppDbContext dbContext)
        {

            _dbContext = dbContext;

        }

        public async Task<List<Setting>> GetSettingData()
        {
            return await _dbContext.Setting.ToListAsync();
        }

        public async Task<List<Setting>> GetUserSettingData(int userId)
        {
            return await _dbContext.Setting
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task AddSettingData(int userId)
        {
            Setting newSetting = new Setting()
            {
                UserId = userId,
                BgColour = "#FFFFFF",
                FontSize = "16"
            };

            _dbContext.Add(newSetting);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeSettingData(int id, string bgColour, string fontSize)
        {
            Setting changedSetting = new Setting()
            {
                Id = id,
                BgColour = bgColour,
                FontSize = fontSize,
                UserId = id
            };

            _dbContext.Update(changedSetting);
            await _dbContext.SaveChangesAsync();
        }
    }
}
