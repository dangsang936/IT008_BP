using System;
using System.IO;
using Newtonsoft.Json;  
namespace DATA
{
    public class DataHelper
    {
        private readonly string jsonFile;
        private GameData data;

        public class GameData
        {
            public int Highscore { get; set; }
        }

        public DataHelper(string file)
        {
            jsonFile = file;

            if (File.Exists(jsonFile))
            {
                var text = File.ReadAllText(jsonFile);
                data = JsonConvert.DeserializeObject<GameData>(text) ?? new GameData { Highscore = 0 };
            }
            else
            {
                data = new GameData { Highscore = 0 };
                Save();
            }
        }

        private void Save()
        {
            File.WriteAllText(jsonFile,
                JsonConvert.SerializeObject(data, Formatting.Indented)
            );
        }

        public int GetHighscore() => data.Highscore;

        public void UpdateHighscore(int score)
        {
            if (score > data.Highscore)
            {
                data.Highscore = score;
                Save();
            }
        }
    }
}
