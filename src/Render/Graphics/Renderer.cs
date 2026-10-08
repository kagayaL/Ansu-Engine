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
    }

    //готовит батч(набор спрайтов) для отправки в гпу
    public void Begin(Matrix? transform = null)
    {
        if (_began)
            throw new InvalidOperationException("Renderer.Begin вызван дважды: батч уже открыт.");

        _batch.Begin(
            //копит все draw чтобы потом отправить в один батч, сортирует если есть слои
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

    public void Clear(Color color)
    {
        _device.Clear(color);
    }

    //текстура, позиция где рисуем, область где рисуем, нулл=весь экран, цвет, нулл=белый
    public void DrawSprite(Texture2D tex, Vector2 pos, Rectangle? src = null, Color? tint = null)
    {
        _batch.Draw(tex, pos, src ?? Rectangle.Empty, tint ?? Color.White);
    }

    //отрисовка спрайта по прямоугольнику
    //переключение мира или камеры

    //освобождение гпу
    public void Dispose()
    {
        _batch.Dispose();
    }
}