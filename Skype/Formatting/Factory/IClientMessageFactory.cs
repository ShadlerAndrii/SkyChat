// Formatting/Factory/IClientMessageFactory.cs
namespace Skype.Formatting.Factory;

using Skype.Formatting;

public interface IClientMessageFactory
{
    // Returns the formatter the client should use (null if none).
    IMessageFormatter? CreateFormatter(string? preferredName = null);
}