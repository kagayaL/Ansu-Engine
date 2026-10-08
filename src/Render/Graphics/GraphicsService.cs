using Microsoft.Xna.Framework.Graphics;

namespace Render.Graphics;

public sealed class GraphicsService
{
    public GraphicsDevice Device { get; }
    public WindowSettings Settings { get; }
    public Renderer Renderer { get; }

    public GraphicsService(GraphicsDevice device, WindowSettings settings)
    {
        Device = device;
        Settings = settings;
        Renderer = new Renderer(device);
    }

    //если изменить размер окна, тут будут меняться все показатели, зависящие от размеров окна
    public void OnSizeChanged(int width, int height)
    {
        
    }
}