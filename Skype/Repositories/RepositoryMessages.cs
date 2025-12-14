using Microsoft.EntityFrameworkCore;
using Skype.Data;
using Skype.Formatting.Factory;
using static Skype.DTOs.MessagesDTO;

namespace Skype.Repositories
{
    public class RepositoryMessages
    {
        AppDbContext _dbContext;
        private readonly IClientMessageFactory _clientFactory;

        public RepositoryMessages(AppDbContext dbContext, IClientMessageFactory clientFactory)
        {
            _dbContext = dbContext;
            _clientFactory = clientFactory;
        }

        public async Task<List<MessageWithItsUserNameDTO>> GetMessageData(int chatId, string? formatName = null)
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

            var formatter = _clientFactory.CreateFormatter(formatName);
            if (formatter != null)
            {
                foreach (var dto in list)
                {
                    var msg = new Message
                    {
                        Id = dto.Id,
                        ChatId = dto.ChatId,
                        OwnerId = dto.OwnerId,
                        TimeSend = dto.TimeSend,
                        Text = dto.Text
                    };
                    dto.Text = formatter.Format(msg);
                }
            }

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
