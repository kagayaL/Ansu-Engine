namespace Core.Time;

public static class Time
{
    public static float DeltaTime { get; private set; }
    public static float TotalTime { get; private set; }
    public static float FPS { get; private set; }

    public static void Update(float deltaTime)
    {
        DeltaTime = deltaTime;
        TotalTime += deltaTime;
        FPS = deltaTime > 0f ? 1f / deltaTime : 0f;
    }
}
