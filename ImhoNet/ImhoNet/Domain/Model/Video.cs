using System.Collections.Generic;

namespace ImhoNet.Domain.Model
{
    public class Video
    {
        private readonly List<UserVideoRate> _rates;
        private readonly List<User> _users;

        public Video(string videoName)
        {
            _rates = new List<UserVideoRate>();
            _users = new List<User>();
            Name = videoName;
        }

        public string Name { get; private set; }

        public User[] Users
        {
            get { return _users.ToArray(); }
        }

        public double GetRate()
        {
            if (_rates.Count == 0) return 0;

            var rate = 0.0;
            for (var i = 0; i < _rates.Count; i++)
            {
                rate += _rates[i].Value;
            }

            return rate / _rates.Count;
        }

        public void AttachRate(UserVideoRate rate)
        {
            if (rate.Value == 0) return;
            _users.Add(rate.User);
            _rates.Add(rate);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}