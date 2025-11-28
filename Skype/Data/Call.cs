using Skype.Constants;

namespace Skype.Data
{
    public class Call : IChatRecord
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int OwnerId { get; set; }
        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }
        public DateTime TimeDuration { get; set; }
        public CallStatus Status { get; set; }

        DateTime IChatRecord.InitDate => TimeStart;
    }
}
