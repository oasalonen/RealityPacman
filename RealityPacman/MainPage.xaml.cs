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
        private GeoCoordinateWatcher watcher;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _engine = new GameEngine();
            _engine.ghostCreated += new GameEngine.GhostCreated(ghostCreated);
            _engine.Start();
            //_engine.Player.Position = new GeoCoordinate(0, 0);

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 20;

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            watcher.Start();
        }

        void ghostCreated(Ghost ghost)
        {
            GhostControl ghostControl = new GhostControl();
            ghostControl.DataContext = ghost;

            positionLayer.AddChild(ghostControl, ghost.Position);
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            // TODO: care about status
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            MapLayer.SetPosition(rect, e.Position.Location);
            map.Center = e.Position.Location;
            _engine.Player.Position = e.Position.Location;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _engine.Player.Name = NavigationContext.QueryString["playerName"];
        } 
    }
}