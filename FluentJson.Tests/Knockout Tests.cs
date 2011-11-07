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
        public void ToViewModel_Simple_Anonymous()
        {
            var anon = new { a = 5, b = "test" };

            var json = Knockout.ToViewModel(anon);

            Assert.AreEqual("{\"a\":ko.observable(5),\"b\":ko.observable(\"test\")}", json.ToJson());
        }

        [TestMethod]
        public void ToViewModel_Anonymous_With_Array()
        {
            var anon = new { a = 5, b = new int[] { 1, 2, 3} };

            var json = Knockout.ToViewModel(anon);

            Assert.AreEqual("{\"a\":ko.observable(5),\"b\":ko.observableArray([1,2,3])}", json.ToJson());
        }

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
