using Skype.Constants;

namespace Skype.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public Setting Setting { get; set; }
        public List<Chat> Chat { get; set; }
    }
}
