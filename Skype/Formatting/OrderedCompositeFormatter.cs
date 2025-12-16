Formatting/OrderedCompositeFormatter.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skype.Data;

namespace Skype.Formatting
{
    // Composite that orders children by FormatterLeaf.Priority (defaults to 0 for non-leaf children).
    // It concatenates outputs and skips disabled leaves or empty outputs.
    public class OrderedCompositeFormatter : IMessageFormatter
    {
        public string Name => "composite-ordered";

        private readonly IReadOnlyList<IMessageFormatter> _children;

        public OrderedCompositeFormatter(IEnumerable<IMessageFormatter> children)
        {
            if (children == null) throw new ArgumentNullException(nameof(children));

            // Filter out possible self-registration and nulls, then order by priority (ascending).
            _children = children
                .Where(c => c != null && c.Name != Name)
                .OrderBy(c => c is FormatterLeaf fl ? fl.Priority : 0)
                .ToList();
        }

        public string Format(Message message)
        {
            var sb = new StringBuilder();
            var first = true;

            foreach (var child in _children)
            {
                // If child is a leaf and disabled, skip it.
                if (child is FormatterLeaf leaf && !leaf.Enabled) continue;

                var part = child.Format(message);
                if (string.IsNullOrEmpty(part)) continue;

                if (!first)
                {
                    sb.AppendLine().AppendLine("---");
                }

                sb.Append(part);
                first = false;
            }

            return sb.ToString();
        }
    }
}