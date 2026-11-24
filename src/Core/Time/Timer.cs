namespace Core.Time;

public class Timer
{
    public float Duration { get; set; }
    public float Elapsed { get; private set; }
    public bool IsLooping { get; set; }
    public bool IsFinished { get; private set; }

    public Timer(float duration, bool isLooping = false)
    {
        Duration = duration;
        IsLooping = isLooping;
        Reset();
    }

    public void Update(float deltaTime)
    {
        if (IsFinished && !IsLooping)
            return;

        Elapsed += deltaTime;
        if (Elapsed >= Duration)
        {
            if (IsLooping)
            {
                Elapsed %= Duration;
            }
            else
            {
                IsFinished = true;
                Elapsed = Duration;
            }
        }
    }

    public void Reset()
    {
        Elapsed = 0f;
        IsFinished = false;
    }
}
