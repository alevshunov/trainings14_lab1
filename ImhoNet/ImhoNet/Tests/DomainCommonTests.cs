using ImhoNet.Domain.Model;
using NUnit.Framework;

namespace ImhoNet.Tests
{
    [TestFixture]
    class DomainCommonTests
    {
        [Test]
        public void ModelObjectsConstructorsShouldFillPropertieses()
        {
            var user = new User("UserA");
            var video = new Video("Video1");
            var rate = new UserVideoRate(7);

            Assert.AreEqual("UserA", user.Name);
            Assert.AreEqual("Video1", video.Name);
            Assert.AreEqual(7, rate.Value);
        }

        [Test]
        public void UserVideoRateShouldAttachedToUserAndVideo()
        {
            var user = new User("A");
            var video = new Video("Video");
            var rate = new UserVideoRate(5);
            rate.AttachTo(user, video);

            Assert.AreEqual(1, user.Videos.Length);
            Assert.AreEqual(1, video.Users.Length);
            Assert.AreSame(user, rate.User);
            Assert.AreSame(video, rate.Video);
            Assert.AreSame(video, user.Videos[0]);
            Assert.AreSame(user, video.Users[0]);
            Assert.AreEqual(rate, user.GetRateForVideo(video));
            Assert.AreEqual(5, user.GetRateForVideo(video).Value);
            Assert.AreEqual(5, video.GetRate());
        }

        [Test]
        public void UserShouldCorrelateToAnotherUser()
        {
            var user1 = new User("A");
            var user2 = new User("B");

            var video1 = new Video("VideoA");
            var video2 = new Video("VideoB");
            var video3 = new Video("VideoC");

            new UserVideoRate(10).AttachTo(user1, video1);
            new UserVideoRate(5).AttachTo(user1, video2);
            new UserVideoRate(8).AttachTo(user2, video1);
            new UserVideoRate(10).AttachTo(user2, video2);
            new UserVideoRate(7).AttachTo(user2, video3);

            double expected = (double)((10 - 8) + (10 - 5)) / 2;

            Assert.AreEqual(expected, user1.GetCorrelationTo(user2));
            Assert.AreEqual(expected, user2.GetCorrelationTo(user1));
            Assert.AreEqual(0, user2.GetCorrelationTo(user2));
            Assert.AreEqual(0, user1.GetCorrelationTo(user1));
        }

        [Test]
        public void VideoShouldHaveAverageRating()
        {
            var video = new Video("Video1");
            var user1 = new User("A");
            var user2 = new User("B");
            var user3 = new User("C");

            new UserVideoRate(10).AttachTo(user1, video);
            new UserVideoRate(6).AttachTo(user2, video);
            new UserVideoRate(6).AttachTo(user3, video);

            var expected = (double)(10 + 6 + 6) / 3;
            Assert.AreEqual(expected, video.GetRate());
        }
    }
}