using Xunit;

using Core.ECS;
using Core.Physics.Systems;
using Core.Physics.Components;
using Core.GameData;
using Core.Math;
using Core.Events;
using Core.Physics;

namespace Tests.RunTime;

public class WorldTest
{
    int forTestCollision = 0;
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

    [Fact]
    public void EntityCollisionResolution()
    {
        World world = new World();
        world.AddSystem(new MovementSystem());
        EventBus eventBus = new EventBus();
        world.AddSystem(new CollisionSystem(eventBus));
        eventBus.Subscribe<CollisionEvent>(HandleTestingCollison);

        Entity entity1 = world.CreateEntity();
        world.AddComponent(entity1, new Transform(0, 0));
        world.AddComponent(entity1, new Velocity(2, 2));
        world.AddComponent(entity1, new Collider(new Rect(0, 0, 10, 10)));

        Entity entity2 = world.CreateEntity();
        world.AddComponent(entity2, new Transform(11, 11));
        world.AddComponent(entity2, new Velocity(0, 0));
        world.AddComponent(entity2, new Collider(new Rect(0, 0, 10, 10)));

        var collider1 = world.GetComponent<Collider>(entity1);
        var collider2 = world.GetComponent<Collider>(entity2);

        world.Update(0);
        Assert.NotNull(collider1);
        Assert.NotNull(collider2);
        Assert.Equal(0, forTestCollision);


        world.Update(3);

        collider1 = world.GetComponent<Collider>(entity1);
        collider2 = world.GetComponent<Collider>(entity2);

        Assert.NotNull(collider1);
        Assert.NotNull(collider2);
        Assert.Equal(2, forTestCollision);

    }

    public void HandleTestingCollison(CollisionEvent ev) // инкрементирует поле forTestCollision когда случается колизия
    {
        forTestCollision++;
    }
}

