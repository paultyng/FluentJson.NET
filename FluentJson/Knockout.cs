using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;
using System.Collections;

namespace FluentJson
{
    public static class Knockout
    {
        public static JsonValue ToViewModel<T>(IContractResolver resolver, T model)
        {
            return ToViewModel(typeof(T), resolver, model);
        }

        public static JsonValue ToViewModel<T>(T model)
        {
            return ToViewModel(typeof(T), null, model);
        }

        public static JsonValue ToViewModel(Type modelType, IContractResolver resolver, object model)
        {
            if (JsonValue.IsPrimitive(resolver, modelType))
            {
                return new JsonValue(resolver, model);
            }
            else
            {
                var dicType = typeof(ViewDataDictionary<>).MakeGenericType(modelType);
                var viewData = (ViewDataDictionary) Activator.CreateInstance(dicType, model);
                var metadata = ModelMetadata.FromStringExpression(string.Empty, viewData);
                var json = JsonObject.Create();

                foreach (var p in metadata.Properties)
                {
                    var vdi = viewData.GetViewDataInfo(p.PropertyName);
                    //not using eval so it doesn't convert to string...
                    var value = vdi.PropertyDescriptor.GetValue(viewData.Model);

                    if (JsonValue.IsPrimitive(resolver, p.ModelType))
                    {
                        json.AddObservable(p.PropertyName, value);
                    }
                    else if (JsonValue.IsArray(resolver, p.ModelType))
                    {
                        //its an enumerable (we test the property type instead of the value incase the value is null)
                        json.AddObservableArray(p.PropertyName, (IEnumerable) value, resolver);                        
                    }
                    else
                    {
                        json.AddProperty(p.PropertyName, ToViewModel(p.ModelType, resolver, value));
                    }
                }

                return json;
            }
        }

        public static JsonObject AddObservableArray(this JsonObject json, string name, IEnumerable array)
        {
            return AddObservableArray(json, name, array, null);
        }

        public static JsonObject AddObservableArray(this JsonObject json, string name, IEnumerable array, IContractResolver resolver)
        {
            if (json == null) throw new ArgumentNullException("json");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            if (array == null) throw new ArgumentNullException("array");

            json.AddProperty(name, new KnockoutObservableArray(resolver, array));

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
            readonly object _array;

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

            public KnockoutObservableArray(IContractResolver resolver, object array)
            {
                if(array == null) throw new ArgumentNullException("array");
                if (!JsonValue.IsArray(resolver, array.GetType())) throw new ArgumentException("array must be a recognized Json.NET array type.", "array");

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
