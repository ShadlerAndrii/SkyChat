using Microsoft.EntityFrameworkCore;
using Skype.Data;
using static Skype.DTOs.MessagesDTO;

namespace Skype.Repositories
{
    public class RepositoryMessages
    {
        AppDbContext _dbContext;

        public RepositoryMessages(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MessageWithItsUserNameDTO>> GetMessageData(int chatId)
        {
            var list = await _dbContext.Messages
                .Where(m => m.ChatId == chatId)
                .Join(
                    _dbContext.Users,
                    message => message.OwnerId,
                    user => user.Id,
                    (message, user) => new MessageWithItsUserNameDTO
                    {
                        Id = message.Id,
                        ChatId = message.ChatId,
                        OwnerId = message.OwnerId,
                        TimeSend = message.TimeSend,
                        Text = message.Text,

                        OwnerName = user.Name,
                    }
                )
                .ToListAsync();

            return list;
        }

        public async Task AddMessageData(int chatId, int ownerId, string text)
        {
            Message newMessage = new Message()
            {
                ChatId = chatId,
                OwnerId = ownerId,
                TimeSend = DateTime.UtcNow,
                Text = text
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
        }
    }
}
