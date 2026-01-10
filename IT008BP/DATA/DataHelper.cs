    using System;
    using System.Data.SQLite;
    using System.IO;

    namespace DATA
    {
        public class DataHelper
        {
            private readonly string dbFile;
            private readonly string connectionString;

            public DataHelper(string file)
            {
                dbFile = file;
                connectionString = $"Data Source={dbFile};Version=3;";

                if (!File.Exists(dbFile))
                {
                    CreateDatabase();
                }
            }

            // Tạo DB + bảng
            private void CreateDatabase()
            {
                SQLiteConnection.CreateFile(dbFile);

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string sql =
                    @"CREATE TABLE GameData (
                        Id INTEGER PRIMARY KEY,
                        Highscore INTEGER NOT NULL
                      );

                      INSERT INTO GameData (Highscore) VALUES (0);";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Lấy highscore
            public int GetHighscore()
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT Highscore FROM GameData LIMIT 1;";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }

            // Cập nhật highscore
            public void UpdateHighscore(int score)
            {
                int current = GetHighscore();
                if (score <= current) return;

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string sql = "UPDATE GameData SET Highscore = @score;";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@score", score);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
