using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FluentJson
{
    public static class Knockout
    {
        public static JsonObject AddObservableArray<T>(this JsonObject json, string name, IEnumerable<T> array)
        {
            return AddObservableArray(json, name, array.Cast<object>());
        }

        public static JsonObject AddObservableArray(this JsonObject json, string name, IEnumerable<object> array)
        {
            if (json == null) throw new ArgumentNullException("json");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            if (array == null) throw new ArgumentNullException("array");

            json.AddProperty(name, new KnockoutObservableArray(array));

            return json;
        }

        public static JsonObject AddObservable(this JsonObject json, string name, object value)
        {
            if (json == null) throw new ArgumentNullException("json");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");

            json.AddProperty(name, new KnockoutObservable(value));

            return json;
        }

        [JsonConverter(typeof(KnockoutObservableArray.Converter))]
        class KnockoutObservableArray
        {
            readonly IEnumerable<object> _array;

            class Converter : JsonConverter<KnockoutObservableArray>
            {
                public override void WriteJson(JsonWriter writer, KnockoutObservableArray value, JsonSerializer serializer)
                {
                    writer.WriteRaw("ko.observableArray(");
                    serializer.Serialize(writer, value._array);
                    writer.WriteRaw(")");
                }

                public override KnockoutObservableArray ReadJson(JsonReader reader, KnockoutObservableArray existingValue, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }
            }

            public KnockoutObservableArray(IEnumerable<object> array)
            {
                _array = array;
            }
        }

        [JsonConverter(typeof(KnockoutObservable.Converter))]
        class KnockoutObservable
        {
            readonly object _value;

            class Converter : JsonConverter<KnockoutObservable>
            {
                public override void WriteJson(JsonWriter writer, KnockoutObservable value, JsonSerializer serializer)
                {
                    writer.WriteRaw("ko.observable(");
                    serializer.Serialize(writer, value._value);
                    writer.WriteRaw(")");
                }

                public override KnockoutObservable ReadJson(JsonReader reader, KnockoutObservable existingValue, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }
            }

            public KnockoutObservable(object value) { _value = value; }
        }
    }
}
