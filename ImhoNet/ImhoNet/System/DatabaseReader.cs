using System;
using System.IO;

namespace ImhoNet.System
{
    public class DatabaseReader
    {
        private string[][] _database;
        private string[] _userNames;
        private string[] _videoNames;
        private int[,] _rates;

        public DatabaseReader(string fileName)
        {
            LoadDatabase(fileName);
            PrepareDatabase();
        }

        public string[] UserNames
        {
            get { return _userNames; }
        }

        public string[] VideoNames
        {
            get { return _videoNames; }
        }

        public int GetRate(string userName, string videoName)
        {
            var userIndex = Array.IndexOf(_userNames, userName);
            var videoIndex = Array.IndexOf(_videoNames, videoName);
            return _rates[videoIndex, userIndex];
        }        
        
        public int GetRate(int userIndex, int videoIndex)
        {
            return _rates[videoIndex, userIndex];
        }

        private void LoadDatabase(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            _database = new string[lines.Length][];
            lines[0] = "# " + lines[0];
            for (var recordIndex = 0; recordIndex < lines.Length; recordIndex++)
            {
                var recordLine = lines[recordIndex].Trim();
                while (recordLine.Contains("  ")) recordLine = recordLine.Replace("  ", " ");
                _database[recordIndex] = recordLine.Split(' ');
            }
        }

        private void PrepareDatabase()
        {
            ParseUsers();
            ParseVideos();
            ParseRates();
        }

        private void ParseUsers()
        {
            _userNames = new string[_database[0].Length - 1];
            Array.Copy(_database[0], 1, _userNames, 0, _database[0].Length - 1);
        }

        private void ParseVideos()
        {
            _videoNames = new string[_database.Length - 1];
            for (var videoIndex = 0; videoIndex < _database.Length - 1; videoIndex++)
            {
                _videoNames[videoIndex] = _database[videoIndex + 1][0];
            }
        }

        private void ParseRates()
        {
            _rates = new int[_videoNames.Length, _userNames.Length];
            for (var videoIndex = 0; videoIndex < _videoNames.Length; videoIndex++)
            {
                for (var userIndex = 0; userIndex < _userNames.Length; userIndex++)
                {
                    _rates[videoIndex, userIndex] = Int32.Parse(_database[videoIndex + 1][userIndex + 1]);
                }
            }
        }
    }
}