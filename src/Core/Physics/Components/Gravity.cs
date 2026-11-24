using Core.ECS;
public struct Gravity : IComponent
{
    public float Force { get; set; }
    public bool IsAffectedByGravity { get; set; }

    public Gravity(float g)
    {
        Force = g;
        IsAffectedByGravity = true;
    }
}
