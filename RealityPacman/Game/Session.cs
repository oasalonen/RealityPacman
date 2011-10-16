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
using System.Device.Location;

namespace RealityPacman.Game
{
    public class Session
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Difficulty Difficulty { get; set; }
        public GeoCoordinate StartCoordinate { get; set; }

        public Session()
        {
            EndTime = new DateTime(0);
            StartTime = new DateTime(0);
            Duration = new TimeSpan(0);
            Difficulty = Difficulty.Easy;
            StartCoordinate = new GeoCoordinate();
        }

        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            EndTime = DateTime.Now;
        }

        public void AddDuration(int milliseconds)
        {
            Duration += new TimeSpan(0, 0, 0, 0, milliseconds);
        }
    }
}
