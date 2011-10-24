using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace FluentJson.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class JsonObject_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProperty_ArgumentException_Object_Overload()
        {
            var json = JsonObject.Create();

            json.AddProperty(null, (object)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProperty_ArgumentException_Action_Overload()
        {
            var json = JsonObject.Create();

            json.AddProperty(null, j => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProperty_ArgumentException_Action_Overload_2()
        {
            var json = JsonObject.Create();

            json.AddProperty("name", (Action<JsonObject>) null);
        }

        [TestMethod]
        public void AddProperty_Child_Object()
        {
            var json = JsonObject.Create();

            json.AddProperty("name", c => c.AddProperty("cname", "value"));

            Assert.AreEqual("{\"name\":{\"cname\":\"value\"}}", json.ToJson());
        }

        [TestMethod]
        public void AddProperty_Null()
        {
            var json = JsonObject.Create();

            json.AddProperty("name", (object)null);

            Assert.AreEqual("{\"name\":null}", json.ToJson());
        }

        [TestMethod]
        public void AddProperty_Multiple_Child_Objects()
        {
            var json = JsonObject.Create();

            json.AddProperty("name"
                , (c,i) => c.AddProperty("cname", "value")
                , (c,i) => c.AddProperty("cname2", "value2"));

            Assert.AreEqual("{\"name\":[{\"cname\":\"value\"},{\"cname2\":\"value2\"}]}", json.ToJson());
        }

        [TestMethod]
        public void ToHtmlString_Test()
        {
            var json = JsonObject.Create();

            json.AddProperty("name", "value");

            var actual = ((IHtmlString)json).ToHtmlString();

            Assert.AreEqual("{\"name\":\"value\"}", actual);
        }
    }
}
