namespace Skype.DTOs
{
    public class ChatsDTO
    {
        public class ChatWithItsUsernamesDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public List<string> Usernames { get; set; }
        }
    }
}
