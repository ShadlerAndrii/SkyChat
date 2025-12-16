// Formatting/ThirdPartyMarkdownAdapter.cs
using Skype.Data;

namespace Skype.Formatting
{
    // Adapter: exposes IMessageFormatter and delegates to ThirdPartyMarkdownFormatter.
    public class ThirdPartyMarkdownAdapter : IMessageFormatter
    {
        public string Name => "thirdparty-md";

        private readonly Formatting.Legacy.ThirdPartyMarkdownFormatter _adaptee;

        // Adaptee injected via DI to keep things testable
        public ThirdPartyMarkdownAdapter(Formatting.Legacy.ThirdPartyMarkdownFormatter adaptee)
        {
            _adaptee = adaptee;
        }

        public string Format(Message message)
        {
            // Adapt IMessageFormatter.Format(Message) to the adaptee's Convert(...)
            return _adaptee.Convert(message.Text, message.OwnerId, message.TimeSend);
        }
    }
}