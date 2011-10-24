using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentJson.Tests
{
    [TestClass]
    public class Knockout_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddObservableArray_ArgumentNullException_Test1()
        {
            var json = JsonObject.Create();
            json.AddObservableArray(null, new int[] {5});
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddObservableArray_ArgumentNullException_Test2()
        {
            JsonObject json = null;
            json.AddObservableArray("test", new int[] { 5 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddObservableArray_ArgumentNullException_Test3()
        {
            var json = JsonObject.Create();
            json.AddObservableArray("test", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddObservable_ArgumentNullException_Test1()
        {
            var json = JsonObject.Create();
            json.AddObservable(null, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddObservable_ArgumentNullException_Test2()
        {
            JsonObject json = null;
            json.AddObservable("test", 5);
        }

        [TestMethod]
        public void AddObservable_Test()
        {
            var json = JsonObject.Create();

            json.AddObservable("test", 5);

            Assert.AreEqual("{\"test\":ko.observable(5)}", json.ToJson());
        }

        [TestMethod]
        public void AddObservableArray_Test()
        {
            var json = JsonObject.Create();

            json.AddObservableArray("test", new int[] { 1, 2 });

            Assert.AreEqual("{\"test\":ko.observableArray([1,2])}", json.ToJson());
        }
    }
}
