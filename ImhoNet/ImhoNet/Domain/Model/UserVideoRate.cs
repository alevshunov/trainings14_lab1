namespace ImhoNet.Domain.Model
{
    public class UserVideoRate
    {
        public const int MaxRate = 10;

        public UserVideoRate(int value)
        {
            Value = value;
        }

        public User User { get; set; }

        public Video Video { get; set; }

        public int Value { get; set; }

        public void AttachTo(User user, Video video)
        {
            User = user;
            Video = video;

            user.AttachRate(this);
            video.AttachRate(this);
        }

        public override string ToString()
        {
            return string.Format("User: {0}, Video: {1}, Value: {2}", User, Video, Value);
        }
    }
}