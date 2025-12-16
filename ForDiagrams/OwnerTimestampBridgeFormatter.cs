// Formatting/Bridge/OwnerTimestampBridgeFormatter.cs
using Skype.Data;

namespace Skype.Formatting.Bridge
{
    // Concrete abstraction: adds owner/time metadata then delegates rendering of body.
    public class OwnerTimestampBridgeFormatter : BridgeMessageFormatter
    {
        public override string Name => "bridge-owner-ts";

        public OwnerTimestampBridgeFormatter(IRenderer renderer) : base(renderer) { }

        public override string Format(Message message)
        {
            var body = _renderer.Render(message);
            return $"[{message.TimeSend:yyyy-MM-dd HH:mm}] {body}";
        }
    }
}