//Formatting/FormatterLeaf.cs
using System;

namespace Skype.Formatting
{
    // Leaf wrapper: provides ordering and enabled flag for existing IMessageFormatter instances.
    public class FormatterLeaf : IMessageFormatter
    {
        private readonly IMessageFormatter _inner;
        private readonly string? _overrideName;

        public int Priority { get; }
        public bool Enabled { get; set; }

        public FormatterLeaf(IMessageFormatter inner, int priority = 0, bool enabled = true, string? name = null)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            Priority = priority;
            Enabled = enabled;
            _overrideName = name;
        }

        // Expose child's name by default, or an override if provided.
        public string Name => _overrideName ?? _inner.Name;

        public string Format(Message message)
        {
            if (!Enabled)
            {
                return string.Empty;
            }

            return _inner.Format(message);
        }
    }
}