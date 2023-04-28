using Native = Newtonsoft.Json.JsonSerializer;

namespace EasyCards.Services
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public static class JsonDeserializer
    {
        private static JsonSerializer serializer;
        private static JsonSerializerSettings settings;

        static JsonDeserializer()
        {
            List<JsonConverter> converters = new();

            converters.Add(new StringEnumConverter());

            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = converters,
            };

            serializer = JsonSerializer.Create(settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
