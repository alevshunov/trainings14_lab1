using System;
using System.Collections.Generic;
using ImhoNet.Domain.Helper;
using ImhoNet.Domain.Model;
using ImhoNet.System;

namespace ImhoNet.Domain
{
    public class Oracle
    {
        private User[] _users;
        private Video[] _videos;
        private Dictionary<string, User> _userByName;
        private Dictionary<string, Video> _videoByName;

        public Oracle(string[] userNames, string[] videoNames)
        {
            PrepareUsers(userNames);
            PrepareVideos(videoNames);
        }

        public User[] Users
        {
            get { return _users; }
        }

        public Video[] Videos
        {
            get { return _videos; }
        }

        public void SetRate(string userName, string videoName, int rateValue)
        {
            var user = _userByName[userName];
            var video = _videoByName[videoName];

            var rate = new UserVideoRate(rateValue);
            rate.AttachTo(user, video);
        }

        public Video GetPredictionFor(string userName)
        {
            var user = _userByName[userName];

            var mostCorrelatedUsersWithCorrelation = GetMostCorrelatedUsersWithCorrelation(user);
            var potentialVideoWithRates = GetPotentialVideos(mostCorrelatedUsersWithCorrelation, user);

            return potentialVideoWithRates.Length == 0 ? null : potentialVideoWithRates[0].Video;
        }

        private void PrepareUsers(string[] userNames)
        {
            _userByName = new Dictionary<string, User>();
            _users = new User[userNames.Length];
            for (var i = 0; i < userNames.Length; i++)
            {
                var userName = userNames[i];
                var user = new User(userName);
                _users[i] = user;
                _userByName[userName] = user;
            }
        }

        private void PrepareVideos(string[] videoNames)
        {
            _videos = new Video[videoNames.Length];
            _videoByName = new Dictionary<string, Video>();
            for (var i = 0; i < videoNames.Length; i++)
            {
                var videoName = videoNames[i];
                var video = new Video(videoName);
                _videos[i] = video;
                _videoByName[videoName] = video;
            }
        }

        private PotentialUser[] GetMostCorrelatedUsersWithCorrelation(User user)
        {
            var correlationUsers = new PotentialUser[_users.Length];

            int userIndex = -1;
            foreach (var comparingUser in _users)
            {
                userIndex++;
                var comparingCorrelation = user.GetCorrelationTo(comparingUser);
                correlationUsers[userIndex] = new PotentialUser(comparingUser, comparingCorrelation);
            }

            Array.Sort(correlationUsers, new PotentialUserComparer());
            return correlationUsers;
        }

        private PotentialVideo[] GetPotentialVideos(PotentialUser[] mostCorrelatedUsersWithCorrelation, User user)
        {
            var potentialVideosWithRates = new List<PotentialVideo>();

            foreach (var userWithCorrelation in mostCorrelatedUsersWithCorrelation)
            {
                var potentialUser = userWithCorrelation.User;
                var potentialUserCorrelation = userWithCorrelation.CorrelationValue;
                var potentialVideos = potentialUser.Videos;
                foreach (var potentialVideo in potentialVideos)
                {
                    if (user.GetRateForVideo(potentialVideo) != null) continue;

                    var potentialVideoUserRate = potentialUser.GetRateForVideo(potentialVideo);
                    var potentialUserWorthRate = (double) potentialVideoUserRate.Value - potentialUserCorrelation;
                    var potentialRateIncludeVideoRate = potentialUserWorthRate + potentialVideo.GetRate()/10;
                    potentialVideosWithRates.Add(new PotentialVideo(potentialVideo, potentialRateIncludeVideoRate));
                }
            }

            var potentialVideoRatesArray = potentialVideosWithRates.ToArray();
            Array.Sort(potentialVideoRatesArray, new PotentialVideoComparer());
            return potentialVideoRatesArray;
        }
    }
}