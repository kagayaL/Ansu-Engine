using Microsoft.Xna.Framework;

namespace Render.Graphics;

public sealed class WindowSettings
{
    public int Width { get; set; } = 1280;
    public int Height { get; set; } = 720;
    public string Title { get; set; } = "Demo";
    public bool AllowResize { get; set; } = false;
    //синхронизация отрисовки с частотой обновления монитора, 60 кадров в секунду
    public bool VSync { get; set; } = true;
    //Update вызывается ровно каждые 1/60 секунды, потом вызывает draw и снова ждет, и так по кругу
    public bool FixedTimeStep { get; set; } = true;
    public Color ClearColor { get; set; } = Color.Black;
}