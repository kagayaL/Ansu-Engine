using Core.ECS;
public struct Gravity : IComponent
{
    public float Force { get; set; }
    public bool IsAffectedByGravity { get; set; }
}
