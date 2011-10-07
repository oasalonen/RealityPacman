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

namespace RealityPacman
{
    public class Ghost
    {
        enum CoarseHeading
        {
            InvalidHeading,
            Vertical,
            Horizontal
        }

        public GeoCoordinate Position { get; set; }
        CoarseHeading _heading;
        Random _random;

        public Ghost(GeoCoordinate position)
        {
            Position = position;
            _heading = CoarseHeading.InvalidHeading;
            _random = new Random();
        }

        public void Process(GeoCoordinate userPosition)
        {
            // Make sure we are processing valid positions
            if (Position.IsUnknown || userPosition.IsUnknown)
            {
                System.Diagnostics.Debug.WriteLine("Warning: Cannot move ghost, user or ghost position is unknown.");
                return;
            }

            MaybeSwitchDirection();
        }

        void MaybeSwitchDirection()
        {
            int rnd;
            switch (_heading)
            {
                case CoarseHeading.InvalidHeading:
                    // Need to generate a new heading
                    // 50/50 random choice
                    rnd = _random.Next(2);
                    if (rnd == 0)
                    {
                        _heading = CoarseHeading.Horizontal;
                    }
                    else
                    {
                        _heading = CoarseHeading.Vertical;
                    }
                    break;
                case CoarseHeading.Vertical:
                case CoarseHeading.Horizontal:
                    // Maybe change direction
                    // Likelihood of changing direction is 2%
                    rnd = _random.Next(100);
                    if (rnd < 2)
                    {
                        if (_heading == CoarseHeading.Horizontal)
                        {
                            _heading = CoarseHeading.Vertical;
                        }
                        else
                        {
                            _heading = CoarseHeading.Horizontal;
                        }
                    }
                    break;
            }
        }
    }
}
