namespace Skype.Data
{
    public class Message : IChatRecord
    {
        public int Id { get; set; }
        public int ChatId {  get; set; }
        public int OwnerId { get; set; }
        public DateTime TimeSend { get; set; }

        public string Text { get; set; }

        DateTime IChatRecord.InitDate => TimeSend;
    }
}
