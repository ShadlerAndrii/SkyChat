// Services/Formatting/IMessageFormatter.cs

// Services/Formatting/IMessageFormatter.cs
using Skype.Data;

namespace Skype.Formatting
{
    public interface IMessageFormatter
    {
        string Name { get; }               // small discriminator for selection
        string Format(Message message);
    }
}