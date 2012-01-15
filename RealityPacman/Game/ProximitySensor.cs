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
using System.Collections.Generic;
using Microsoft.Devices;

namespace GhostMaps.Game
{
    public class ProximitySensor
    {
        enum Proximity
        {
            NextTo = 0,
            Close = 12,
            Nearby = 20,
            Far = 40,
            Infinity = 10000000
        }

        private Proximity _proximity;
        private VibrateController _vibrator;

        public ProximitySensor() 
        {
            _vibrator = VibrateController.Default;
            _proximity = Proximity.Infinity;
        }

        public void Process(ICollection<Ghost> ghosts, Player player)
        {
            double minDistance = Double.PositiveInfinity;
            foreach (Ghost g in ghosts)
            {
                minDistance = Math.Min(minDistance, g.DistanceToPlayer);
            }

            try
            {
                if (minDistance < (double) _proximity)
                {
                    if (minDistance < (double)Proximity.Close)
                    {
                        _proximity = Proximity.NextTo;
                        _vibrator.Start(new TimeSpan(0, 0, 0, 0, 1000));
                    }
                    else if (minDistance < (double)Proximity.Nearby)
                    {
                        _proximity = Proximity.Close;
                        _vibrator.Start(new TimeSpan(0, 0, 0, 0, 500));
                    }
                    else if (minDistance < (double)Proximity.Far)
                    {
                        _proximity = Proximity.Nearby;
                        _vibrator.Start(new TimeSpan(0, 0, 0, 0, 100));
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                System.Diagnostics.Debug.WriteLine("Invalid vibration duration");
            }
        }
    }
}
