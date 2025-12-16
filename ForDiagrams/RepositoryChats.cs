using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Skype.Data;
using static Skype.DTOs.ChatsDTO;

namespace Skype.Repositories
{
    public class RepositoryChats
    {
        AppDbContext _dbContext;

        public RepositoryChats(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<List<User>> GetListOfUsers(List<string> usernames)
        {
            //var users = await _dbContext.Users
            //    .Where(u => usersId.Contains(u.Id))
            //    .ToListAsync();

            var users = await _dbContext.Users
                .Where(u => usernames.Contains(u.Username.ToLower()))
                .ToListAsync();

            return users;
        }

        public async Task<List<ChatWithItsUsernamesDTO>> GetChatData(int userId)
        {
            return await _dbContext.Chats
                .Where(c => c.User.Any(u => u.Id == userId))
                .Select(c => new ChatWithItsUsernamesDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Usernames = c.User.Select(u => u.Username).ToList()
                })
                .ToListAsync();
        }

        public async Task AddChatData(string name, string description, List<string> usernames)
        {
            Chat newChat = new Chat()
            {
                Name = name,
                Description = description,
                User = await GetListOfUsers(usernames)
            };

            _dbContext.Chats.Add(newChat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeChatData(int id, string name, string description, List<string> usernames)
        {
            var chat = await _dbContext.Chats
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (chat == null)
            {
                return;
            }

            chat.Name = name;
            chat.Description = description;

            usernames = usernames
                .Select(u => u.ToLower())
                .ToList();

            var currentUsernames = chat.User
                .Select(u => u.Username.ToLower())
                .ToList();

            var usersToRemove = chat.User
                .Where(u => !usernames.Contains(u.Username.ToLower()))
                .ToList();

            var usernamesToAdd = usernames
                .Except(currentUsernames)
                .ToList();

            var usersToAdd = await _dbContext.Users
                .Where
                (
                    u => usernamesToAdd
                    .Contains(u.Username.ToLower())
                )
                .ToListAsync();

            foreach (var user in usersToRemove)
            {
                chat.User.Remove(user);
            }

            foreach (var user in usersToAdd)
            {
                chat.User.Add(user);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveChatData(int id)
        {
            await _dbContext.Database
                .ExecuteSqlRawAsync
                (
                    "DELETE FROM ChatUser WHERE ChatId = {0}",
                    id
                );

            await _dbContext.Messages
                .Where(m => m.ChatId == id)
                .ExecuteDeleteAsync();

            await _dbContext.Chats
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}