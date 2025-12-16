using Skype.Data;

namespace Skype.DTOs
{
    public class MessagesDTO
    {
        public class MessageWithItsUserNameDTO
        {
            public int Id { get; set; }
            public int ChatId { get; set; }
            public int OwnerId { get; set; }
            public DateTime TimeSend { get; set; }

            public string Text { get; set; }

            public string OwnerName { get; set; }
        }
    }
}
