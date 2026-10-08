using Xunit;
using Core.ECS;
using Core.Physics.Systems;
using Core.Physics.Components;

namespace Tests.RunTime;

public class WorldTest
{
    [Fact]
    public void Mfve()
    {

        World world = new World();
        Entity deniska = world.CreateEntity();
        Assert.Equal(1, deniska.Id);

        world.AddSystem(new MovmentSystem());
        world.AddComponent(deniska, new Position(33, 33));

        world.AddComponent(deniska, new Velocity(11, 11));
        world.Update(1);
        var res = world.GetComponent<Position>(deniska);
        Console.WriteLine(res.ToString());
        Assert.Equal(true, res.Equals(new Position(44, 44)));

    }
}

