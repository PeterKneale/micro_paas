namespace Control.Infrastructure;

using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(string))]
public partial class AotJsonContext : JsonSerializerContext
{
}