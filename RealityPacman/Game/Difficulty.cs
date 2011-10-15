using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RealityPacman.Game
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class DifficultySettings
    {
        public Difficulty Level { get; set; }

        public DifficultySettings()
        {
            Level = Difficulty.Easy;
        }

        public DifficultySettings(Difficulty level)
        {
            Level = level;
        }

        public DifficultySettings(int level)
        {
            Level = (Difficulty) level;
        }

        public int ToInt()
        {
            return (int)Level;
        }
    }
}
