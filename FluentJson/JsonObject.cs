using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace FluentJson
{
    [JsonConverter(typeof(JsonObject.Converter))]
    public sealed class JsonObject : JsonValue
    {
        Dictionary<string, object> _properties = new Dictionary<string,object>();
        
        class Converter : JsonConverter<JsonObject>
        {
            public override void WriteJson(JsonWriter writer, JsonObject value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value._properties);
            }

            public override JsonObject ReadJson(JsonReader reader, JsonObject existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        private JsonObject() : base(t => ((JsonObject)t)._properties)
        {
        }

        public JsonObject AddProperty(string name, params Action<JsonObject, int>[] childBuilders)
        {
            var children = new List<JsonObject>();

            for (var i = 0; i < childBuilders.Length; i++)
            {
                var childBuilder = childBuilders[i];
                var json = JsonObject.Create();
                childBuilder(json, i);
                children.Add(json);
            }

            _properties.Add(name, children);

            return this;
        }

        public JsonObject AddProperty(string name, Action<JsonObject> childBuilder)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            if (childBuilder == null) throw new ArgumentNullException("childBuilder");

            var json = JsonObject.Create();
            childBuilder(json);
            _properties.Add(name, json);
            return this;
        }

        public JsonObject AddProperty(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");

            _properties.Add(name, value);
            return this;
        }

        public static JsonObject Create()
        {
            return new JsonObject();
        }
    }
}
