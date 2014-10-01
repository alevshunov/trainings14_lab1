using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImhoNet.Domain;
using Microsoft.SqlServer.Server;
using NUnit.Framework;

namespace ImhoNet.Tests
{
    [TestFixture]
    class OracleTests
    {
        [Test]
        public void OracleShouldPredictMostPotentialVideo()
        {
            var oracle = new Oracle(new []{"A", "B", "C"}, new[]{"Video1", "Video2", "Video3", "Video4", "Video5"});

            oracle.SetRate("A", "Video1", 10);
            oracle.SetRate("A", "Video2", 8);
            oracle.SetRate("A", "Video3", 6);
            oracle.SetRate("A", "Video4", 4);
            oracle.SetRate("A", "Video5", 2);

            oracle.SetRate("B", "Video1", 7);
            oracle.SetRate("B", "Video2", 8);
            oracle.SetRate("B", "Video3", 0);
            oracle.SetRate("B", "Video4", 9);
            oracle.SetRate("B", "Video5", 6);
            
            oracle.SetRate("C", "Video1", 0);
            oracle.SetRate("C", "Video2", 8);
            oracle.SetRate("C", "Video3", 0);
            oracle.SetRate("C", "Video4", 0);
            oracle.SetRate("C", "Video5", 6);

            var video = oracle.GetPredictionFor("C");

            Assert.AreEqual(3, oracle.Users.Length);
            Assert.AreEqual(5, oracle.Videos.Length);
            Assert.AreEqual(10, oracle.Users[0].GetRateForVideo(oracle.Videos[0]).Value);
            Assert.AreEqual(8, oracle.Users[0].GetRateForVideo(oracle.Videos[1]).Value);
            Assert.AreEqual(6, oracle.Users[0].GetRateForVideo(oracle.Videos[2]).Value);
            Assert.AreEqual(4, oracle.Users[0].GetRateForVideo(oracle.Videos[3]).Value);
            Assert.AreEqual(2, oracle.Users[0].GetRateForVideo(oracle.Videos[4]).Value);
            Assert.AreEqual(7, oracle.Users[1].GetRateForVideo(oracle.Videos[0]).Value);
            Assert.AreEqual(8, oracle.Users[1].GetRateForVideo(oracle.Videos[1]).Value);
            Assert.AreEqual(null, oracle.Users[1].GetRateForVideo(oracle.Videos[2]));
            Assert.AreEqual(9, oracle.Users[1].GetRateForVideo(oracle.Videos[3]).Value);
            Assert.AreEqual(6, oracle.Users[1].GetRateForVideo(oracle.Videos[4]).Value);
            Assert.AreEqual(null, oracle.Users[2].GetRateForVideo(oracle.Videos[0]));
            Assert.AreEqual(8, oracle.Users[2].GetRateForVideo(oracle.Videos[1]).Value);
            Assert.AreEqual(null, oracle.Users[2].GetRateForVideo(oracle.Videos[2]));
            Assert.AreEqual(null, oracle.Users[2].GetRateForVideo(oracle.Videos[3]));
            Assert.AreEqual(6, oracle.Users[2].GetRateForVideo(oracle.Videos[4]).Value);
            
            Assert.AreEqual("Video4", video.Name);

        }
    }
}
