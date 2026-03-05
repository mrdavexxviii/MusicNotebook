using MusicNotebook.NotebookDefinitions;
using MusicNotebook.Serialisation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MusicNotebookLibrary.Serialisation
{
    public static class JsonHandler
    {
        public static Notebook FromJson(string jsonString)
        {

            return JsonSerializer.Deserialize<Notebook>(jsonString, GetJsonOptions());
        }

        public static string ToJson(Notebook notebook)
        {
            return JsonSerializer.Serialize(notebook,GetJsonOptions());
        }


        private static JsonSerializerOptions GetJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()

            };

            options.Converters.Add(new NotebookPageCollectionConverter());
            options.Converters.Add(new StrokeCollectionJsonConverter());
            options.MakeReadOnly();
            return options;
        }
    }
}
