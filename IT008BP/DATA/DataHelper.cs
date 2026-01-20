using System;
using System.Data.SQLite;
using System.IO;

namespace DATA
{
    public class DataHelper
    {
        private readonly string dbFile;
        private readonly string connectionString;

        //khởi tạo cơ sở dữ liệu SQLite, tạo thư mục lưu trữ an toàn và tự động tạo database khi chạy game lần đầu.
        public DataHelper(string fileName = "gamescore.db")
            {
                string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appFolder = Path.Combine(baseDir, "BlockPuzzle");

                if (!Directory.Exists(appFolder))
                    Directory.CreateDirectory(appFolder);

                dbFile = Path.Combine(appFolder, fileName);
                connectionString = $"Data Source={dbFile};Version=3;";

                if (!File.Exists(dbFile))
                    CreateDatabase();
            }
        //tạo cơ sở dữ liệu
        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(dbFile);

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS GameData (
                            Id INTEGER PRIMARY KEY CHECK(Id = 1),
                            Highscore INTEGER NOT NULL
                        );";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"
                        INSERT OR IGNORE INTO GameData (Id, Highscore)
                        VALUES (1, 0);";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //trả về điểm cao nhất
        public int GetHighscore()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(
                    "SELECT Highscore FROM GameData WHERE Id = 1;", conn))
                {
                    object result = cmd.ExecuteScalar();
                    return result == null ? 0 : Convert.ToInt32(result);
                }
            }
        }
        //cập nhật điểm cao nhất
        public void UpdateHighscore(int score)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(
                    @"UPDATE GameData 
                      SET Highscore = @score 
                      WHERE Id = 1 AND @score > Highscore;", conn))
                {
                    cmd.Parameters.AddWithValue("@score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
