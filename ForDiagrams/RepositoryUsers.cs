using Microsoft.EntityFrameworkCore;
using Skype.Constants;
using Skype.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Skype.Repositories
{
    public class RepositoryUsers
    {
        AppDbContext _dbContext;

        RepositorySettings _settings;

        public RepositoryUsers(AppDbContext dbContext, RepositorySettings repositorySettings)
        {
            _dbContext = dbContext;
            _settings = repositorySettings;
        }

        private string ConvertPasswordToHash(string password)
        {
            // Генеруємо salt (16 байт)
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Рахуємо хеш PBKDF2(SHA256, 100k iterations)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 32 байти = 256 біт

            // Зберігаємо разом: salt + hash → Base64
            byte[] hashBytes = new byte[48]; // 16 байт salt + 32 байт hash
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string savedHash, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(savedHash);

            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

            byte[] storedHash = new byte[32];
            Buffer.BlockCopy(hashBytes, 16, storedHash, 0, 32);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hashToCheck = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(storedHash, hashToCheck);
        }

        public async Task<List<User>> GetUserData()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> TryAddUserData( string name,
                                                string username,
                                                string phone,
                                                string password,
                                                UserRole role)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
            {
                return false;
            }

            User newUser = new User()
            {
                Name = name,
                Username = username,
                Phone = phone,
                Password = ConvertPasswordToHash(password),
                Role = role
            };

            _dbContext.Add(newUser);
            await _dbContext.SaveChangesAsync();

            await _settings.AddSettingData(newUser.Id);

            return true;
        }

        public async Task<User> LoginUser(  string username,
                                            string password)
        {
            var user = await _dbContext.Users                
                .SingleOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            /* 
            var a = await _dbContext.Users.Where(u => u.Username == username)
                .Include(s => s.Setting)
                .Select(s => new Setting()
                {
                    FontSize = s.Setting.FontSize,
                    BgColour = s.Setting.BgColour,
                })
                .ToListAsync();
            */

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(user.Password, password))
            {
                return null;
            }

            return user;
        }



        //public async Task<List<Chat>> GetChatData(int userId)
        //{
        //    var list = await _dbContext.Users
        //        .Where(u => u.Id ==  userId)
        //        .SelectMany(u => u.Chat)
        //        .ToListAsync();

        //    return list;
        //}
    }
}