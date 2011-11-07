using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using Newtonsoft.Json.Serialization;

namespace FluentJson
{
    public class JsonValue : IHtmlString
    {
        Func<JsonValue, object> _getValue;

        public JsonValue(object value) : this(null, value) { }

        public JsonValue(IContractResolver resolver, object value)
        {
            if (value != null && !IsPrimitive(resolver, value.GetType()))
            {
                throw new ArgumentException("value must be a primitive recognized by Json.NET", "value");
            }

            _getValue = t => value;
        }

        protected JsonValue(Func<JsonValue, object> getValue)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            _getValue = getValue;
        }

        public static bool IsArray(IContractResolver resolver, Type type)
        {
            if (resolver == null)
            {
                //get default from json serializer (since no public access to default resolver instance)
                resolver = new JsonSerializer().ContractResolver;
            }

            var contract = resolver.ResolveContract(type);

            return contract is JsonArrayContract;
        }

        public static bool IsPrimitive(IContractResolver resolver, Type type)
        {
            if (resolver == null)
            {
                //get default from json serializer (since no public access to default resolver instance)
                resolver = new JsonSerializer().ContractResolver;
            }

            var contract = resolver.ResolveContract(type);

            return contract is JsonPrimitiveContract;
        }
        
        public string ToJson()
        {
            var sw = new StringWriter();
            new JsonSerializer().Serialize(sw, _getValue(this));
            return sw.GetStringBuilder().ToString();
        }

        string IHtmlString.ToHtmlString()
        {
            return ToJson();
        }

        public override string ToString()
        {
            return ToJson();
        }
    }
}
