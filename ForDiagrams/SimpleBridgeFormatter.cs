// Formatting/Bridge/SimpleBridgeFormatter.cs
using Skype.Data;

namespace Skype.Formatting.Bridge
{
    // Concrete abstraction: simple pass-through to renderer.
    public class SimpleBridgeFormatter : BridgeMessageFormatter
    {
        public override string Name => "bridge-simple";

        public SimpleBridgeFormatter(IRenderer renderer) : base(renderer) { }

        // Inherits Format -> renderer.Render(message)
    }
}