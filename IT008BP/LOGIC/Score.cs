using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA;

namespace LOGIC
{
    public class Score
    {
        public int ScoreValue;
        public static int ComboCount;
        public int HighScore;

        private int NoClearTurns;
        private const int MaxNoClearTurns = 5;

        public Score()
        {
            ScoreValue = 0;
            ComboCount = 1;
            HighScore = 0;
            NoClearTurns = 0;
        }

        public int CalculatePoints(int blockSize, int linesCleared)
        {
            int pointsFromBlocks = blockSize;   
            int pointsFromLines = 36 * linesCleared;

            int totalPoints = (pointsFromBlocks + pointsFromLines) * ComboCount;
            return totalPoints;
        }

        public void ProcessTurn(int blockSize, int linesCleared)
        {
            int pointsThisTurn = CalculatePoints(blockSize, linesCleared);
            ScoreValue += pointsThisTurn;

            if (linesCleared > 0)
            {
                ComboCount++;
                NoClearTurns = 0;
            }
            else
            {
                NoClearTurns++;
                if (NoClearTurns >= MaxNoClearTurns)
                {
                    ComboCount = 1;
                    NoClearTurns = 0;
                }
            }

            if (ScoreValue > HighScore)
                HighScore = ScoreValue;
        }

        public void PerfectClear()
        {
            ScoreValue += 300;

            ComboCount++;
            NoClearTurns = 0;

            if (ScoreValue > HighScore)
                HighScore = ScoreValue;
        }

        public void Reset()
        {
            ScoreValue = 0;
            ComboCount = 1;
            NoClearTurns = 0;
        }
    }
}
