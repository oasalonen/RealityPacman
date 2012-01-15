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
using Microsoft.Devices.Sensors;

namespace RealityPacman
{
    public partial class MainPage : PhoneApplicationPage
    {
        Engine _engine;
        private GeoCoordinateWatcher _watcher;
        private Compass _compass;

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

            if (Compass.IsSupported)
            {
                _compass = new Compass();
                _compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(compassChanged);
                _compass.Start();
            }

            gpsHideAnimation.Completed += new EventHandler(gpsHideAnimation_Completed);

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
        }

        void gameOver(Session session)
        {
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
            MessageBox.Show("Game over! " + durationString);
            NavigationService.GoBack();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            gpsStatusBox.Status = e.Status;

            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    setGpsStatusBarVisibility(Visibility.Visible);
                    break;
                case GeoPositionStatus.Initializing:
                    setGpsStatusBarVisibility(Visibility.Visible);
                    break;
                case GeoPositionStatus.NoData:
                    setGpsStatusBarVisibility(Visibility.Visible);
                    break;
                case GeoPositionStatus.Ready:
                    setGpsStatusBarVisibility(Visibility.Collapsed);
                    break;
            }
        }

        GeoCoordinate lastPlayerPosition=null;

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (_engine != null)
            {
                MapLayer.SetPosition(rect, e.Position.Location);
                map.Center = e.Position.Location;
                _engine.Player.Position = e.Position.Location;

                if (lastPlayerPosition != null && _compass == null)
                {
                    double angle = Math.Atan2(lastPlayerPosition.Latitude - e.Position.Location.Latitude, e.Position.Location.Longitude - lastPlayerPosition.Longitude);
                    rect.turn(angle);
                }
                lastPlayerPosition = e.Position.Location;
                rect.Visibility = Visibility.Visible;

                // Only start engine when getting the first location
                _engine.Start();
            }
        }

        private void turnPlayer(double angle)
        {
            rect.turn((angle - 90) * Math.PI / 180);

        }

        private void compassChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            CompassReading reading = (CompassReading)e.SensorReading;
            Dispatcher.BeginInvoke(() => { turnPlayer(reading.TrueHeading); });
        }

        private void setGpsStatusBarVisibility(Visibility visibility)
        {
            if (gpsStatusBox.Visibility == visibility)
            {
                return;
            }

            if (gpsStatusBox.Visibility == Visibility.Collapsed)
            {
                gpsStatusBox.Visibility = visibility;
                gpsHideAnimation.Stop();
                gpsShowAnimation.Begin();
            }
            else if (gpsStatusBox.Visibility == Visibility.Visible)
            {
                gpsShowAnimation.Stop();
                gpsHideAnimation.Begin();
            }
        }

        private void endGameYesButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                _watcher.Stop();
                _engine.Stop();
                _watcher = null;
                _engine = null;
                NavigationService.GoBack();
            }
        }

        private void endGameNoButton_Click(object sender, RoutedEventArgs e)
        {
            endGameYesNoAnimation.Stop();
            endGameHideAnimation.Begin();
        }

        private void gpsHideAnimation_Completed(object sender, EventArgs e)
        {
            gpsStatusBox.Visibility = Visibility.Collapsed;
        }

        private void endGameYesNoAnimation_Completed(object sender, EventArgs e)
        {
            endGamePrompt.Visibility = Visibility.Collapsed;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (endGamePrompt.Visibility == Visibility.Visible)
            {
                if (endGameHideAnimation.GetCurrentState() != ClockState.Active)
                {
                    endGameYesNoAnimation.Stop();
                    endGameHideAnimation.Begin();
                }
                else
                {
                    endGameHideAnimation.Stop();
                    endGameYesNoAnimation.Begin();
                }
            }
            else
            {
                endGamePrompt.Visibility = Visibility.Visible;
                endGamePrompt.Opacity = 0.6;
                endGameYesNoAnimation.Begin();
            }
            e.Cancel = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                int difficultyInt = Int32.Parse(NavigationContext.QueryString["difficulty"]);
                _engine.Difficulty = (Difficulty)difficultyInt;
                _watcher.Start();
            }
        }

        private void gpsStatusBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (gpsStatusBox.Status == GeoPositionStatus.Disabled)
            {
                NavigationService.Navigate(new Uri("/GpsHelpPage.xaml?helpTopic=disabledPositioning", UriKind.Relative));
            }
            else if (gpsStatusBox.Status == GeoPositionStatus.NoData)
            {
                NavigationService.Navigate(new Uri("/GpsHelpPage.xaml?helpTopic=noPosition", UriKind.Relative));
            }
        }
    }
}