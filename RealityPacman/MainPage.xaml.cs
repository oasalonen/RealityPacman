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
using RealityPacman.Game;

namespace RealityPacman
{
    public partial class MainPage : PhoneApplicationPage
    {
        Engine _engine;
        private GeoCoordinateWatcher _watcher;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _engine = new Engine();
            _engine.ghostCreated += new Engine.GhostCreated(ghostCreated);
            _engine.worldObjectCreated += new Engine.WorldObjectCreated(worldObjectCreated);
            _engine.worldObjectRemoved += new Engine.WorldObjectRemoved(worldObjectRemoved);
            _engine.ghostsMoved += new Engine.GhostsMoved(ghostsMoved);
            _engine.gameStarted += new Engine.GameStarted(gameStarted);
            _engine.gameOver += new Engine.GameOver(gameOver);
            //_engine.Player.Position = new GeoCoordinate(0, 0);

            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _watcher.MovementThreshold = 0;

            _watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            _watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            rect.Visibility = Visibility.Collapsed;
        }

        void ghostCreated(Ghost ghost)
        {
            GhostControl ghostControl = new GhostControl();
            ghostControl.DataContext = ghost;

            positionLayer.AddChild(ghostControl, ghost.Position);
        }

        void worldObjectCreated(WorldObject worldObject)
        {
            MapItemControl itemControl = new MapItemControl();
            itemControl.DataContext = worldObject;

            itemLayer.AddChild(itemControl, worldObject.Position);
        }

        void worldObjectRemoved(WorldObject worldObject)
        {
            foreach (UIElement e in itemLayer.Children)
            {
                MapItemControl mapItem = e as MapItemControl;
                if (mapItem != null)
                {
                    if (mapItem.DataContext == worldObject)
                    {
                        itemLayer.Children.Remove(e);
                        return;
                    }
                }
            }
        }

        void ghostsMoved()
        {
            // Kludge for the problem that ghosts don't update their position
            for (int i = positionLayer.Children.Count - 1; i > 0; i--)
            {
                GhostControl child = positionLayer.Children.ElementAt(i) as GhostControl;
                if (child != null)
                {
                    positionLayer.Children.Remove(child);
                }
                positionLayer.AddChild(child, (child.DataContext as Ghost).Position);
            }
        }

        void gameStarted()
        {
            App.IsIdleModeEnabled = false;
        }

        void gameOver(Session session)
        {
            App.IsIdleModeEnabled = true;

            _watcher.Stop();

            // Save session to database
            App.ViewModel.AddSession(new Models.SessionModel(session));

            // Show game duration in a message box
            TimeSpan duration = session.Duration;
            String durationString = "You lasted ";
            if (duration.Hours >= 1.0)
            {
                durationString += (int)duration.Hours + " h " + (int)duration.Minutes + " min ";
            }
            else if (duration.Minutes >= 1.0)
            {
                durationString += (int)duration.Minutes + " min ";
            }
            durationString += (int)duration.Seconds + " s.";
            MessageBox.Show("Game over, " + _engine.Player.Name + "! " + durationString);
            NavigationService.GoBack();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            // TODO: indicate status
        }

        GeoCoordinate lastPlayerPosition=null;

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            MapLayer.SetPosition(rect, e.Position.Location);
            map.Center = e.Position.Location;
            _engine.Player.Position = e.Position.Location;

            if (lastPlayerPosition != null)
            {
                double angle = Math.Atan2( lastPlayerPosition.Latitude -e.Position.Location.Latitude, e.Position.Location.Longitude - lastPlayerPosition.Longitude);
                rect.turn(angle);
            }
            lastPlayerPosition = e.Position.Location;
            rect.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int difficultyInt = Int32.Parse(NavigationContext.QueryString["difficulty"]);
            _engine.Difficulty = (Difficulty) difficultyInt;
            _engine.Start();
            _watcher.Start();
        } 
    }
}