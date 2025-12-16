// Services/Formatting/PlainTextFormatter.cs
using Skype.Data;

namespace Skype.Formatting
{
    public class PlainTextFormatter : IMessageFormatter
    {
        public string Name => "plain";

        public string Format(Message message)
        {
            // minimal, safe formatting
            return $"{message.TimeSend:yyyy-MM-dd HH:mm} [{message.OwnerId}]: {message.Text}";
        }
    }
}