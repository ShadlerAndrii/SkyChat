// Formatting/Factory/WebClientMessageFactory.cs
using System.Linq;
using Skype.Formatting;

namespace Skype.Formatting.Factory;

public class WebClientMessageFactory : IClientMessageFactory
{
    private readonly IEnumerable<IMessageFormatter> _formatters;

    // Inject available formatters (registered in DI)
    public WebClientMessageFactory(IEnumerable<IMessageFormatter> formatters)
    {
        _formatters = formatters;
    }

    public IMessageFormatter? CreateFormatter(string? preferredName = null)
    {
        if (string.IsNullOrEmpty(preferredName))
        {
            // default for web: prefer markdown if available
            return _formatters.FirstOrDefault(f => f.Name == "md") 
                ?? _formatters.FirstOrDefault();
        }

        return _formatters.FirstOrDefault(f => f.Name == preferredName)
            ?? _formatters.FirstOrDefault();
    }
}