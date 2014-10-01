using System.Collections.Generic;
using NUnit.Framework;

namespace ImhoNet.Tests
{
    class TestObject
    {
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return Name == null ? 0 : Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TestObject)) return false;

            return Name == ((TestObject)obj).Name;
        }
    }

    [TestFixture]
    class ListIndexOfTests
    {
        [Test]
        public void TestObjectsShouldBeEquals()
        {
            var objA = new TestObject() { Name = "Hello" };
            var objB = new TestObject() { Name = "Hello" };
            Assert.AreEqual(objA, objB);
        }
        
        [Test]
        public void TestObjectsShouldNotBeEquals()
        {
            var objA = new TestObject() { Name = "Hello" };
            var objB = new TestObject() { Name = "Hello1" };
            Assert.AreNotEqual(objA, objB);
        }

        [Test]
        public void IndexOfCommonTests()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject());
            list.Add(new TestObject() { Name = "Hello" });
            list.Add(new TestObject() { Name = "Hello1" });

            Assert.AreEqual(-1, list.IndexOf(null));
            Assert.AreEqual(-1, list.IndexOf(new TestObject() { Name = "Hello2" }));
            Assert.AreEqual(0, list.IndexOf(new TestObject()));
            Assert.AreEqual(1, list.IndexOf(list[1]));
            Assert.AreEqual(1, list.IndexOf(new TestObject() { Name = "Hello" }));
        }

        [Test]
        public void TwoStringsShouldBeEquals()
        {
            Assert.AreEqual("Hello", "Hello");
        }        
    }
}