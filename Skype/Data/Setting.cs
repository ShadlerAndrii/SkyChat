namespace Skype.Data
{
    public class Setting
    {
        public int Id { get; set; }
        public string BgColour {  get; set; }
        public string FontSize { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
