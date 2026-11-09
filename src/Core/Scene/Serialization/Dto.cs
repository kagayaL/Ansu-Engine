using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Scene.Serialization;

public sealed class SceneData
{
    [JsonPropertyName("schema_version")]
    public int SchemaVersion { get; set; } = 1;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("source_level_path")]
    public string? SourceLevelPath { get; set; }

    [JsonPropertyName("entities")]
    public List<EntityData> Entities { get; set; } = new();
}

public sealed class EntityData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("components")]
    public List<ComponentData> Components { get; set; } = new();
}

public sealed class ComponentData
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public object? Data { get; set; }
}
