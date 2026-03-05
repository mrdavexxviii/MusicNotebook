using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Ink;

namespace MusicNotebookLibrary.Serialisation
{
    /// <summary>
    /// JSON converter that serializes a <see cref="StrokeCollection"/> to a Base64-encoded ISF blob
    /// and deserializes back by constructing a new <see cref="StrokeCollection"/> from the stream.
    /// </summary>
    public sealed class StrokeCollectionJsonConverter : JsonConverter<StrokeCollection>
    {
        public override StrokeCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            // Expect a base64 string containing the ISF data
            var base64 = reader.GetString();
            if (string.IsNullOrEmpty(base64))
                return new StrokeCollection();

            var bytes = Convert.FromBase64String(base64);
            using var ms = new MemoryStream(bytes, writable: false);
            return new StrokeCollection(ms);
        }

        public override void Write(Utf8JsonWriter writer, StrokeCollection? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            using var ms = new MemoryStream();
            value.Save(ms);
            var bytes = ms.ToArray();
            writer.WriteBase64StringValue(bytes);
        }
    }
}
