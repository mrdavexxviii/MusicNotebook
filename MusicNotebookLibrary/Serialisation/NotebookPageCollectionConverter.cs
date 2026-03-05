using MusicNotebook.NotebookDefinitions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicNotebook.Serialisation;

public class NotebookPageCollectionConverter : JsonConverter<IList<INotebookPage>>
{
    public override IList<INotebookPage> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = new List<INotebookPage>();
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var jsonDocument = JsonDocument.ParseValue(ref reader);
                var root = jsonDocument.RootElement;

                // Use a discriminator property or type name to determine the concrete type
                if (root.TryGetProperty("type", out var typeProp) && typeProp.GetString() == nameof(TextPage))
                {
                    TextPage? elementA = JsonSerializer.Deserialize<TextPage>(jsonDocument, options);
                    if (elementA != null)
                    {
                        list.Add(elementA);
                    }
                }
                else if (root.TryGetProperty("type", out typeProp) && typeProp.GetString() == nameof(ImagePage))
                {
                    var elementB = JsonSerializer.Deserialize<ImagePage>(jsonDocument, options);
                    if (elementB != null)
                    {
                        list.Add(elementB);
                    }
                }
                else
                {
                    // Fallback: try to deserialize as IElement (if no discriminator)
                    var element = JsonSerializer.Deserialize<INotebookPage>(jsonDocument, options);
                    if (element != null)
                    {
                        list.Add(element);
                    }
                }
            }
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, IList<INotebookPage> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
        writer.WriteEndArray();
    }
}