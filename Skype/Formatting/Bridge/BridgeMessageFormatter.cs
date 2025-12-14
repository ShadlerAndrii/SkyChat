// Formatting/Bridge/BridgeMessageFormatter.cs
using Skype.Data;

namespace Skype.Formatting.Bridge
{
    using Skype.Formatting;

    // Abstraction side of the Bridge: base formatter holding a renderer.
    public abstract class BridgeMessageFormatter : IMessageFormatter
    {
        protected readonly IRenderer _renderer;

        protected BridgeMessageFormatter(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public abstract string Name { get; }

        // High-level formatting uses the renderer (implementation) to produce final string.
        public virtual string Format(Message message)
        {
            return _renderer.Render(message);
        }
    }
}