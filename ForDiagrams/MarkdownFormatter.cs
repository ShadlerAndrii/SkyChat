// Services/Formatting/MarkdownFormatter.cs
using Skype.Data;
using System.Net;

namespace Skype.Formatting
{
    public class MarkdownFormatter : IMessageFormatter
    {
        public string Name => "md";

        public string Format(Message message)
        {
            // basic markdown example — escape text
            var text = WebUtility.HtmlEncode(message.Text);
            return $"**{message.OwnerId}** _{message.TimeSend:HH:mm}_  \n{text}";
        }
    }
}