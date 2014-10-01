using System;
using System.Collections.Generic;

namespace ImhoNet.Domain.Model
{
    public class User
    {
        private readonly List<UserVideoRate> _rates;
        private readonly List<Video> _videos;

        public User(string userName)
        {
            _rates = new List<UserVideoRate>();
            _videos = new List<Video>();
            Name = userName;
        }

        public string Name { get; private set; }

        public Video[] Videos
        {
            get { return _videos.ToArray(); }
        }

        public void AttachRate(UserVideoRate rate)
        {
            if (rate.Value == 0) return;
            _videos.Add(rate.Video);
            _rates.Add(rate);
        }

        public UserVideoRate GetRateForVideo(Video video)
        {
            foreach (var videoRate in _rates)
            {
                if (videoRate.Video.Name == video.Name)
                    return videoRate;
            }

            return null;
        }

        public double GetCorrelationTo(User user)
        {
            var correlation = 0.0;
            var collisionsCount = 0;

            foreach (var videoRate in _rates)
            {
                var video = videoRate.Video;

                var comparingUserRate = user.GetRateForVideo(video);
                if (comparingUserRate == null) continue;

                double comparingCorrelation = Math.Abs(videoRate.Value - comparingUserRate.Value);
                correlation += comparingCorrelation;
                collisionsCount++;
            }

            if (collisionsCount == 0) return double.MaxValue;

            correlation = correlation/collisionsCount;

            return correlation;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}