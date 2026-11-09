using Microsoft.Xna.Framework;

namespace Render.Camera;

//есть три режима работы камеры для трех демок: 
//слежение, блокирование комнатой и вертикальное слежение
public enum FollowMode
{
    Follow,
    RoomLocked,
    VerticalFollow
}

//
public class CameraFollow
{
    private readonly Camera2d _camera;
    private readonly int _screenHeight;
    private readonly int _screenWidth;

    //текущий мод по умолчанию слежение
    public FollowMode Mode { get; set; } = FollowMode.Follow;
    
    // плавность слежения 
    public float Smoothness { get; set; } = 0.1f;

    //границы мира (комнаты), если null то нет ограничений
    public Rectangle? WorldBounds { get; set; }

    //фиксированная координата Х для вертикального слежения
    public float FixedX { get; set; }
    
    public CameraFollow (Camera2d camera, int screenHeight, int screenWidth)
    {
        _camera = camera;
        _screenHeight = screenHeight;
        _screenWidth = screenWidth;
    }

    //реализуем слежение для каждого мода
    //обновление логики слежения кадра, обновляется каждый кадр, 
    //deltaTime - время, которое прошло с прошлого кадра
    //60f - нормализуем плавность к 60 фпс
    public void Update (Vector2 targetPosition, float deltaTime)
    {
        float smoothToFps = Smoothness * deltaTime * 60f;
        switch (Mode)
        {
            case FollowMode.Follow:
            _camera.Position = Vector2.Lerp(_camera.Position, targetPosition, smoothToFps);
            break;

            case FollowMode.RoomLocked:
            _camera.Position = targetPosition;
            if (WorldBounds.HasValue)
            {                
                //чтобы края экрана не выходили за границы WorldBounds
                float visibleHalfWidth = (_screenWidth / 2f) / _camera.Zoom;
                float visibleHalfHeight = (_screenHeight / 2f) / _camera.Zoom;
        
                float minX = WorldBounds.Value.Left + visibleHalfWidth;
                float maxX = WorldBounds.Value.Right - visibleHalfWidth;
                float minY = WorldBounds.Value.Top + visibleHalfHeight;
                float maxY = WorldBounds.Value.Bottom - visibleHalfHeight;

                _camera.Position = new Vector2(
                    MathHelper.Clamp(_camera.Position.X, minX, maxX),
                    MathHelper.Clamp(_camera.Position.Y, minY, maxY)
                );
            }
            break;

            case FollowMode.VerticalFollow:
            _camera.Position = new Vector2(
                FixedX,
                MathHelper.Lerp(_camera.Position.Y, targetPosition.Y, Smoothness * deltaTime * 60)
            );
            break;
        }
    }
}