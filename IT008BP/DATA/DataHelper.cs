using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DATA
{
    //Bang Player 
    public class Player
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int Highscore { get; set; }
        public List<GameScore> Scores { get; set; } = new List<GameScore>();

    }
    //Bang GameScore
    public class GameScore
    {
        public int ScoreID { get; set; }
        public int Score { get; set; }
        public DateTime PlayDate { get; set; }
    }

    public class DataHelper
    {
        private readonly string jsonFile;
        private List<Player> players;

        public DataHelper(string file)
        {
            jsonFile = file;
            if (File.Exists(jsonFile))
            {
                var text = File.ReadAllText(jsonFile);
                players = JsonSerializer.Deserialize<List<Player>>(text) ?? new List<Player>();
            }
            else
            {
                players = new List<Player>();
            }
        }


        private void Save() => File.WriteAllText(jsonFile, JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true }));

        public int CreateUser(string username)
        {
            if (players.Any(p => p.Username == username))
                throw new Exception("Username đã tồn tại!");

            Random rnd = new Random();
            int userId;
            do
            {
                userId = rnd.Next(100000, 999999);
            } while (players.Any(p => p.UserID == userId));

            players.Add(new Player { UserID = userId, Username = username, Highscore = 0 });
            Save();
            return userId;
        }

        public int AddScore(int userId, int score)
        {
            var player = players.FirstOrDefault(p => p.UserID == userId);
            if (player == null) throw new Exception("UserID không tồn tại!");

            int turn = player.Scores.Count + 1;
            int scoreId = userId * 100 + turn;

            player.Scores.Add(new GameScore { ScoreID = scoreId, Score = score, PlayDate = DateTime.Now });
            player.Highscore = Math.Max(player.Highscore, score);

            Save();
            return scoreId;
        }

        public Player GetUserInfo(int userId)
        {
            var player = players.FirstOrDefault(p => p.UserID == userId);
            if (player == null) throw new Exception("UserID không tồn tại!");
            return player;
        }

        public List<Player> GetLeaderboard()
        {
            return players.OrderByDescending(p => p.Highscore).Take(100).ToList();
        }
    }
}
