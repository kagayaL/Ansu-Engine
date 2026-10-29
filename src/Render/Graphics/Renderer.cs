using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Render.Graphics;

//обещание, что в классе есть айдиизпозабл
public sealed class Renderer : IDisposable
{
    //через графикдевайс общаемся с gpu, 
    //он умеет: создавать текстуры, рисовать примитивы, управлять состояниями
    //и отправлять кадр на экран - создает моногейм
    private readonly GraphicsDevice _device;
    private readonly SpriteBatch _batch;
    private Texture2D _pixelTexture;
    private bool _began;

    public Renderer(GraphicsDevice device)
    {
        _device = device;
        //при увеличении или уменьшении текстуры движок берет цвет ближайшего пикселя без сглаживания,
        //по дефолту в моногейм стоит linerclamp который сглаживает и замыливает картинку
        _device.SamplerStates[0] = SamplerState.PointClamp;
        //глубина для камеры в 3д, нам не нужен
        _device.DepthStencilState = DepthStencilState.None;
        //превращает треугольники в пиксели, как бы идет по пикселям и определяет этому спрайту(треугольнику) 
        //он пренадлежит или нет, кулнан потому что 2д, чтобы при повороте картинке или при движинии ничего не потерялось,
        //чтобы все спрайты отрисовывалис и ничего не отрезалось(как в 3д, из-за углов камеры и для экономии гпу)
        _device.RasterizerState = RasterizerState.CullNone;
        _batch = new SpriteBatch(device);
        _pixelTexture = new Texture2D(device, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    //готовит батч(набор спрайтов) для отправки в гпу
    public void Begin(Matrix? transform = null)
    {
        if (_began)
            throw new InvalidOperationException("Renderer.Begin вызван дважды: батч уже открыт.");

        _batch.Begin(
            //отрисовывает слои начиная с задних
            sortMode: SpriteSortMode.Deferred,
            //смешивание цветов, так чтобы прозрачные пиксели были видны
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            //типичный шейдер без всякий спецэффектов
            effect: null,
            //каждая вершина спрайта умножается на эту матрицу
            //если матрица — камера, то спрайты рисуются в мировых координатах(с учетом угла камеры)
            transformMatrix: transform);

        _began = true;
    }

    public void End()
    {
        if (!_began) return;
        _batch.End();
        _began = false;
    }

    public void Clear(Color? color = null)
    {
        Color eraseColor = color ?? Color.Transparent;
        _device.Clear(eraseColor);
    }

    //текстура, позиция где рисуем фигуру в настоящем размере, область где рисуем, нулл=весь экран, цвет, нулл=белый
    public void DrawSprite(Texture2D tex, Vector2 pos, Rectangle? src = null, Color? tint = null)
    {
        _batch.Draw(tex, pos, src, tint ?? Color.White);
    }

    //отрисовка спрайта по прямоугольнику, т.е ограничиваем область отрисовки
    public void DrawSprite(Texture2D tex, Rectangle destination, Rectangle? src = null, Color? tint = null)
    {
        _batch.Draw(tex, destination, src, tint ?? Color.White);
    }

    //pисует сплошной цветной прямоугольник
    public void DrawRect(Rectangle destination, Color color)
    {
        _batch.Draw(_pixelTexture, destination, color);
    }

    public void DrawSprite(Texture2D tex, Vector2 pos, float rotation, Vector2? origin = null, Rectangle? src = null, 
    Vector2? scale = null, Color? tint = null,  SpriteEffects effects = SpriteEffects.None, float layerDepth = 0f)
    {
        Vector2 actualOrigin = origin ?? new Vector2(0.5f, 0.5f);
        Vector2 actualScale = scale ?? Vector2.One;
        int spriteWidth = src.HasValue ? src.Value.Width : tex.Width;
        int spriteHeight = src.HasValue ? src.Value.Height : tex.Height;
        Vector2 originInPixels = new Vector2(actualOrigin.X * spriteWidth, actualOrigin.Y * spriteHeight);
        _batch.Draw(texture: tex, position: pos, sourceRectangle: src, color: tint ?? Color.White,
        rotation: rotation, origin: originInPixels, scale: actualScale, effects: effects, layerDepth: layerDepth);
    }

    //освобождение гпу
    public void Dispose()
    {
        _batch.Dispose();
        _pixelTexture.Dispose();
    }
}