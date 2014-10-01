using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImhoNet.Domain.Model;
using ImhoNet.System;

namespace ImhoNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseReader = new DatabaseReader("database.txt");
            var oracle = Bootstrapper.BuildOracleInstance(databaseReader);

            var userName = databaseReader.UserNames[databaseReader.UserNames.Length - 1];
            var video = oracle.GetPredictionFor(userName);

            Render(video);
        }


        private static void Render(Video video)
        {
            var result = new StringBuilder();
            if (video == null)
            {
                result.AppendLine("Nothing ...");
            }
            else
            {
                result.AppendFormat("Video: {0} (common rate is {1})", video.Name, video.GetRate());
                result.AppendLine();
                foreach (var user in video.Users)
                {
                    result.AppendFormat("{1} points by user {0}.", user.Name, user.GetRateForVideo(video).Value);
                    result.AppendLine();
                }
            }

            File.WriteAllText("result.txt", result.ToString());
        }
    }
}
