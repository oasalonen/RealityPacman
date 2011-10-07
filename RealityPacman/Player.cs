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
    public class Player
    {
        String _name;
        public GeoCoordinate Position { get; set; }
        public String Name
        {
            get
            {
                if (_name == null)
                {
                    return "Anonymous";
                }
                else
                {
                    return _name;
                }
            }
            set
            {
                _name = value;
            }
        }
    }
}
