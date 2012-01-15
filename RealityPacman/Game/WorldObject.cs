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
using System.ComponentModel;

namespace GhostMaps.Game
{
    public class WorldObject : INotifyPropertyChanged
    {
        const double CollisionDistanceThreshold = 10.0;

        protected GeoCoordinate _position;
        public GeoCoordinate Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    NotifyPropertyChanged("Position");
                }
            }
        }

        public WorldObject()
        {
        }

        public WorldObject(GeoCoordinate position)
        {
            Position = position;
        }

        public bool CollidesWith(WorldObject worldObject)
        {
            return Position.GetDistanceTo(worldObject.Position) < CollisionDistanceThreshold;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
