using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImhoNet.Domain.Helper;
using ImhoNet.Domain.Model;
using ImhoNet.System;
using NUnit.Framework;

namespace ImhoNet.Tests
{
    [TestFixture]
    class SystemTests
    {
        [Test]
        public void UserWithLessCorrelationRateShouldBeFirst()
        {
            var comparer = new PotentialUserComparer();

            var a = new PotentialUser(new User("A"), 0.1);
            var b = new PotentialUser(new User("B"), 0.9);

            Assert.AreEqual(-1, comparer.Compare(a, b));
            Assert.AreEqual(1, comparer.Compare(b, a));
            Assert.AreEqual(0, comparer.Compare(b, b));
            Assert.AreEqual(0, comparer.Compare(a, a));
        }

        [Test]
        public void VideoWithBiggerPotentialRateShouldBeFirst()
        {
            var comparer = new PotentialVideoComparer();
            
            var a = new PotentialVideo(new Video("VA"), 0.1);
            var b = new PotentialVideo(new Video("VB"), 0.9);

            Assert.AreEqual(1, comparer.Compare(a, b));
            Assert.AreEqual(-1, comparer.Compare(b, a));
            Assert.AreEqual(0, comparer.Compare(b, b));
            Assert.AreEqual(0, comparer.Compare(a, a));
        }

        [Test]
        public void DatabaseReaderShouldReadDataFromFile()
        {
            var data = "  A   B   C\nV1  1  2  3\nV2 4  5  6";
            File.WriteAllText("tests.tmp", data);
            var reader = new DatabaseReader("tests.tmp");

            Assert.AreEqual(3, reader.UserNames.Length);
            Assert.AreEqual(2, reader.VideoNames.Length);
            Assert.AreEqual(new[]{"A", "B", "C"}, reader.UserNames);
            Assert.AreEqual(new[]{"V1", "V2"}, reader.VideoNames);
            Assert.AreEqual(1, reader.GetRate("A", "V1"));
            Assert.AreEqual(2, reader.GetRate("B", "V1"));
            Assert.AreEqual(3, reader.GetRate("C", "V1"));
            Assert.AreEqual(4, reader.GetRate("A", "V2"));
            Assert.AreEqual(5, reader.GetRate("B", "V2"));
            Assert.AreEqual(6, reader.GetRate("C", "V2"));
            Assert.AreEqual(1, reader.GetRate(0, 0));
            Assert.AreEqual(2, reader.GetRate(1, 0));
            Assert.AreEqual(3, reader.GetRate(2, 0));
            Assert.AreEqual(4, reader.GetRate(0, 1));
            Assert.AreEqual(5, reader.GetRate(1, 1));
            Assert.AreEqual(6, reader.GetRate(2, 1));

            File.Delete("tests.tmp");
        }

        [Test]
        public void BootstrapperShouldBuildOracleObject()
        {
            var data = "  A   B   C\nV1  1  2  3\nV2 4  0  6";
            File.WriteAllText("tests.tmp", data);
            var reader = new DatabaseReader("tests.tmp");

            var oracle = Bootstrapper.BuildOracleInstance(reader);

            Assert.AreEqual(3, oracle.Users.Length);
            Assert.AreEqual(2, oracle.Videos.Length);
            Assert.AreEqual("A", oracle.Users[0].Name);
            Assert.AreEqual("B", oracle.Users[1].Name);
            Assert.AreEqual("C", oracle.Users[2].Name);
            Assert.AreEqual("V1", oracle.Videos[0].Name);
            Assert.AreEqual("V2", oracle.Videos[1].Name);
            Assert.AreEqual(1, oracle.Users[0].GetRateForVideo(oracle.Videos[0]).Value);
            Assert.AreEqual(2, oracle.Users[1].GetRateForVideo(oracle.Videos[0]).Value);
            Assert.AreEqual(3, oracle.Users[2].GetRateForVideo(oracle.Videos[0]).Value);
            Assert.AreEqual(4, oracle.Users[0].GetRateForVideo(oracle.Videos[1]).Value);
            Assert.AreEqual(null, oracle.Users[1].GetRateForVideo(oracle.Videos[1]));
            Assert.AreEqual(6, oracle.Users[2].GetRateForVideo(oracle.Videos[1]).Value);

            Assert.AreEqual(2, oracle.Users[0].Videos.Length);
            Assert.AreEqual(1, oracle.Users[1].Videos.Length);
            Assert.AreEqual(2, oracle.Users[2].Videos.Length);
            
            Assert.AreEqual(3, oracle.Videos[0].Users.Length);
            Assert.AreEqual(2, oracle.Videos[1].Users.Length);
            
            Assert.AreEqual(oracle.Videos[0], oracle.Users[0].Videos[0]);
            Assert.AreEqual(oracle.Videos[0], oracle.Users[1].Videos[0]);
            Assert.AreEqual(oracle.Videos[0], oracle.Users[2].Videos[0]);
            Assert.AreEqual(oracle.Videos[1], oracle.Users[0].Videos[1]);
            Assert.AreEqual(oracle.Videos[1], oracle.Users[2].Videos[1]);
        }

    }
}
