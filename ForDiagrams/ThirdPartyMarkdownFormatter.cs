// Formatting/ThirdPartyMarkdownFormatter.cs
namespace Skype.Formatting.Legacy
{
    // Simulates a third?party / legacy formatter with a different API.
    public class ThirdPartyMarkdownFormatter
    {
        public string Convert(string text, int ownerId, DateTime time)
        {
            // Simulated conversion (escape and decorate) — replace with real library call.
            var escaped = System.Net.WebUtility.HtmlEncode(text);
            return $"**User:{ownerId}** _{time:yyyy-MM-dd HH:mm}_\n\n{escaped}";
        }
    }
}