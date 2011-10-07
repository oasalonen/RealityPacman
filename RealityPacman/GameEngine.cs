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
using System.Windows.Threading;

namespace RealityPacman
{
    public class GameEngine
    {
        public enum GameDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        DispatcherTimer _gameTimer;
        const int _tickInterval = 500; // Engine ticks every 500 ms
        public GameDifficulty Difficulty { get; set; }

        public GameEngine()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = new TimeSpan(0, 0, 0, 0, _tickInterval);
            _gameTimer.Tick += new EventHandler(_gameTimer_Tick);
        }

        public void Start()
        {
            _gameTimer.Start();
        }

        public void Stop()
        {
            _gameTimer.Stop();
        }

        public void _gameTimer_Tick(Object sender, EventArgs e)
        {
            // Process each ghost
        }
    }
}
