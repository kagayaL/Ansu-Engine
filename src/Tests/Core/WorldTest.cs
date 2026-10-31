using Xunit;

using Core.ECS;
using Core.Physics.Systems;
using Core.Physics.Components;
using Core.GameData;

namespace Tests.RunTime;

public class WorldTest
{
    [Fact]
    public void MoveEntity()
    {
        World world = new World();
        Entity deniska = world.CreateEntity();
        Assert.Equal(1, deniska.Id);

        world.AddSystem(new MovementSystem());
        world.AddComponent(deniska, new Transform(33, 33));

        world.AddComponent(deniska, new Velocity(11, 11));
        world.Update(1);
        var res = world.GetComponent<Transform>(deniska);
        Console.WriteLine(res.ToString());
        Assert.Equal(true, res.Equals(new Transform(44, 44)));

    }

    [Fact]
    public void QueueOfRenderedEntities()
    {
        World world = new World();
        Entity deniska = world.CreateEntity();
        world.AddComponent<Transform>(deniska, new Transform(33, 33));

        var emptyList = new List<Entity>();
        Assert.Equal(emptyList, world.GetRenderList()); // если у сущности только одно из двух (Position, Sprite), она не попадает в очередь

        world.AddComponent<Sprite>(deniska, new Sprite());

        var entityList = new List<Entity> { deniska };
        Assert.Equal(entityList, world.GetRenderList()); // когда у deniska появился Sprite, он входит в очредеь на отрисовку

        world.RemoveComponent<Transform>(deniska);
        Assert.Equal(emptyList, world.GetRenderList()); // при удалении какого-нибудь из компонентов (Position, Sprite), сущность удаляется из очереди
    }


}

