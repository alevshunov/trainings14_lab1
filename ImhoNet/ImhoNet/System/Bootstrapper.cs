using ImhoNet.Domain;

namespace ImhoNet.System
{
    internal class Bootstrapper
    {
        public static Oracle BuildOracleInstance(DatabaseReader databaseReader)
        {
            var videos = databaseReader.VideoNames;
            var users = databaseReader.UserNames;
            var oracle = new Oracle(users, videos);
            ApplyRates(users, videos, databaseReader, oracle);
            return oracle;
        }

        private static void ApplyRates(string[] users, string[] videos, DatabaseReader databaseReader, Oracle oracle)
        {
            for (var videoIndex = 0; videoIndex < videos.Length; videoIndex++)
            {
                for (var userIndex = 0; userIndex < users.Length; userIndex++)
                {
                    var rate = databaseReader.GetRate(userIndex, videoIndex);
                    if (rate == 0) continue;
                    oracle.SetRate(users[userIndex], videos[videoIndex], rate);
                }
            }
        }
    }
}