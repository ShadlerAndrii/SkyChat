// Formatting/Bridge/MarkdownRenderer.cs
using Skype.Data;
using System.Net;

namespace Skype.Formatting.Bridge
{
    // Markdown renderer (low-level implementation).
    public class MarkdownRenderer : IRenderer
    {
        public string Render(Message message)
        {
            // Minimal example: escape and wrap as markdown
            var text = WebUtility.HtmlEncode(message.Text);
            return $"**{message.OwnerId}**\n\n{text}";
        }
    }
}