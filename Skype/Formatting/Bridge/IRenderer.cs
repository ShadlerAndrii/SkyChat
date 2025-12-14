// Formatting/Bridge/IRenderer.cs
namespace Skype.Formatting.Bridge
{
    using Skype.Data;

    // Implementation side of the Bridge: low-level rendering engines.
    public interface IRenderer
    {
        string Render(Message message);
    }
}