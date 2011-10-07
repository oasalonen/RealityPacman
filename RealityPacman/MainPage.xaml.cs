using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;

namespace RealityPacman
{
    public partial class MainPage : PhoneApplicationPage
    {
        GameEngine _engine;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _engine = new GameEngine();
            _engine.Start();
            //_engine.Player.Position = new GeoCoordinate(0, 0);
        }
    }
}