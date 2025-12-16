//Formatting/CompositeMessageFormatter.cs
using System.Linq;
using System.Text;
using Skype.Data;

namespace Skype.Formatting
{
    // Composite: implements IMessageFormatter and delegates to many IMessageFormatter children.
    public class CompositeMessageFormatter : IMessageFormatter
    {
        public string Name => "composite-all";

        private readonly IEnumerable<IMessageFormatter> _children;

        public CompositeMessageFormatter(IEnumerable<IMessageFormatter> children)
        {
            // exclude self by name if DI registered together
            _children = (children ?? Enumerable.Empty<IMessageFormatter>())
                .Where(c => c != null && c.Name != Name)
                .ToList();
        }

        public string Format(Message message)
        {
            var sb = new StringBuilder();
            var first = true;

            foreach (var child in _children)
            {
                if (!first) sb.AppendLine().AppendLine("---"); // visual separator
                sb.Append(child.Format(message));
                first = false;
            }

            return sb.ToString();
        }
    }
}