// Formatting/Bridge/TextRenderer.cs
using Skype.Data;
using System.Net;

namespace Skype.Formatting.Bridge
{
    // Simple plain-text renderer (low-level implementation).
    public class TextRenderer : IRenderer
    {
        public string Render(Message message)
        {
            // Escape text minimally and return full text body
            var text = WebUtility.HtmlEncode(message.Text);
            return $"{text}";
        }
    }
}