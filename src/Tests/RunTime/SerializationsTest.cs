using Runtime;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class LayerDataTest
{
    public static void Main()
    {
        // 1. Создаём объект вручную
        var original = new LayerData
        {
            Grid = new[] { 1, 1, 1, 1, 0, 0, 1, 1 },
            Rows = 2,
            Cols = 4,
            TileSize = 32,
            Objects = new List<LayerObject>
            {
                new LayerObject
                {
                    Type = 1,
                    X = 10.5f,
                    Y = 20.0f,
                    Properties = new Dictionary<string, string>
                    {
                        { "name", "player" },
                        { "hp", "100" }
                    }
                },
                new LayerObject
                {
                    Type = 2,
                    X = 100.0f,
                    Y = 50.5f,
                    Properties = new Dictionary<string, string>
                    {
                        { "name", "enemy" },
                        { "damage", "10" }
                    }
                }
            }
        };

        // 2. Настройки сериализатора
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,           // красиво форматировать
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };

        // 3. Сериализация в JSON
        string json = JsonSerializer.Serialize(original, options);
        Console.WriteLine("=== Сериализованный JSON ===");
        Console.WriteLine(json);

        // 4. Десериализация обратно
        var restored = JsonSerializer.Deserialize<LayerData>(json, options);

        Console.WriteLine("\n=== Проверка ===");
        Console.WriteLine($"Rows: {restored!.Rows}, Cols: {restored.Cols}");
        Console.WriteLine($"Objects count: {restored.Objects.Count}");
        Console.WriteLine($"First object name: {restored.Objects[0].Properties["name"]}");

        // 5. Простая валидация
        bool ok = restored.Rows == original.Rows
               && restored.Cols == original.Cols
               && restored.Objects.Count == original.Objects.Count;

        Console.WriteLine(ok ? "✅ Всё работает!" : "❌ Ошибка!");
    }
}