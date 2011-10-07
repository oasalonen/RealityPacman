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
using RealityPacman.Routing;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;

namespace RealityPacman
{
    public class Ghost : INotifyPropertyChanged
    {
        enum CoarseHeading
        {
            InvalidHeading,
            Latitude,
            Longitude
        }

        const double Epsilon = 0.000001;
        const double LatitudeSpeed = 0.000008;
        const double LongitudeSpeed = 0.000008;

        private ObservableCollection<Location> _wayPoints;

        private Waypoint StartPosition = new Waypoint();

        private GeoCoordinate _position;
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
        CoarseHeading _heading;
        Random _random;

        public Ghost(GeoCoordinate position)
        {
            Position = position;
            StartPosition.Location = position;
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
            if (_wayPoints == null || _wayPoints.Count == 0)
            {
                MaybeSwitchDirection(userPosition);
                Move(userPosition);
            }
            else
            {
                double oldDiff = MoveToLastWayPoint(_wayPoints[0]);
                if (oldDiff < 0.00005) _wayPoints.RemoveAt(0);
            }
        }

        private void CalculateRouting(GeoCoordinate userPosition)
        {
            Waypoint endPosition = new Waypoint();
            endPosition.Location = userPosition;

            var routeRequest = new RouteRequest();

            routeRequest.Credentials = new Credentials();
            routeRequest.Credentials.ApplicationId = "Al9X2Bk2UP07iOZG9N_pt4yGUWpLHcyGmG5EjiRSZDFi4EJn6AnF6MxtGfehDJZi";
            routeRequest.Waypoints = new System.Collections.ObjectModel.ObservableCollection<Waypoint>();
            routeRequest.Waypoints.Add(StartPosition);
            routeRequest.Waypoints.Add(endPosition);
            routeRequest.Options = new RouteOptions();
            routeRequest.Options.RoutePathType = RoutePathType.Points;
            routeRequest.UserProfile = new UserProfile();
            routeRequest.UserProfile.DistanceUnit = DistanceUnit.Kilometer;

            var routeClient = new RouteServiceClient("BasicHttpBinding_IRouteService");
            routeClient.CalculateRouteCompleted += new EventHandler<CalculateRouteCompletedEventArgs>(routeClient_CalculateRouteCompleted);
            routeClient.CalculateRouteAsync(routeRequest);
        }

        void MaybeSwitchDirection(GeoCoordinate userPosition)
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
                        _heading = CoarseHeading.Longitude;
                    }
                    else
                    {
                        _heading = CoarseHeading.Latitude;
                    }
                    break;
                case CoarseHeading.Latitude:
                case CoarseHeading.Longitude:
                    // Check if there only exists one direction to the user
                    if (Math.Abs(userPosition.Latitude - Position.Latitude) < Epsilon)
                    {
                        _heading = CoarseHeading.Longitude;
                    }
                    else if (Math.Abs(userPosition.Longitude - Position.Longitude) < Epsilon)
                    {
                        _heading = CoarseHeading.Latitude;
                    }
                    else
                    {
                        // Two possible directions, maybe change direction
                        // Likelihood of changing direction is 2%
                        rnd = _random.Next(100);
                        if (rnd < 2)
                        {
                            if (_heading == CoarseHeading.Longitude)
                            {
                                _heading = CoarseHeading.Latitude;
                            }
                            else
                            {
                                _heading = CoarseHeading.Longitude;
                            }
                        }
                    }
                    break;
            }
        }

        void Move(GeoCoordinate userPosition)
        {
            System.Diagnostics.Debug.Assert(_heading != CoarseHeading.InvalidHeading);

            double diff;
            switch (_heading)
            {
                case CoarseHeading.Longitude:
                    diff = userPosition.Latitude - Position.Latitude;
                    Position.Latitude += Math.Sign(diff) * LatitudeSpeed;
                    NotifyPropertyChanged("Position");
                    break;
                case CoarseHeading.Latitude:
                    diff = userPosition.Longitude - Position.Longitude;
                    Position.Longitude += Math.Sign(diff) * LongitudeSpeed;
                    NotifyPropertyChanged("Position");
                    break;
            }
        }

        private double MoveToLastWayPoint(Location location)
        {
            double diff;
            diff = location.Latitude - Position.Latitude;
            Position.Latitude += Math.Sign(diff) * LatitudeSpeed;
            return diff;
        }

        void routeClient_CalculateRouteCompleted(object sender, CalculateRouteCompletedEventArgs e)
        {
            _wayPoints = e.Result.Result.RoutePath.Points;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
