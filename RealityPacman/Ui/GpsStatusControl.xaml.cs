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
using System.Windows.Threading;
using System.Device.Location;
using System.Windows.Media.Imaging;

namespace RealityPacman
{
    public partial class GpsStatusControl : UserControl
    {
        private DispatcherTimer _animationTimer;
        private int _frameCount = 0;
        private int _frame = 0;
        private int _delta = 1;

        private GeoPositionStatus _gpsStatus;
        public GeoPositionStatus Status
        {
            get { return _gpsStatus; }
            set
            {
                _frame = 0;
                _gpsStatus = value;
                Canvas.SetLeft(sprite, 0);

                switch (value)
                {
                    case GeoPositionStatus.Disabled:
                        gpsStatusLabel.Text = "Please enable positioning on this device";
                        sprite.Source = new BitmapImage(new Uri("/RealityPacman;component/Images/satellite-disabled.png", UriKind.RelativeOrAbsolute));
                        _frameCount = 0;
                        _animationTimer.Stop();
                        break;
                    case GeoPositionStatus.Initializing:
                    case GeoPositionStatus.NoData:
                        gpsStatusLabel.Text = "Looking for your position";
                        sprite.Source = new BitmapImage(new Uri("/RealityPacman;component/Images/satellite-sprite.png", UriKind.RelativeOrAbsolute));
                        _frameCount = 11;
                        _animationTimer.Start();
                        break;
                    case GeoPositionStatus.Ready:
                        gpsStatusLabel.Text = "Position acquired";
                        sprite.Source = new BitmapImage(new Uri("/RealityPacman;component/Images/satellite-ready.png", UriKind.RelativeOrAbsolute));
                        _frameCount = 0;
                        _animationTimer.Stop();
                        break;
                }
            }
        }

        public GpsStatusControl()
        {
            InitializeComponent();

            _animationTimer = new DispatcherTimer();
            _animationTimer.Interval = TimeSpan.FromMilliseconds(100);
            _animationTimer.Tick += new EventHandler(animationTimer_Tick);
        }

        void animationTimer_Tick(object sender, EventArgs e)
        {
            if (_frameCount == 0)
            {
                return;
            }

            _frame += _delta;
            if (_frame == _frameCount - 1)
            {
                _delta = -1;
            }
            else if (_frame == 0)
            {
                _delta = 1;
            }
            Canvas.SetLeft(sprite, -64 * _frame);
        }
    }
}
