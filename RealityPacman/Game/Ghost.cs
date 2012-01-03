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

namespace RealityPacman.Game
{
    public class Ghost : WorldObject
    {
        enum CoarseHeading
        {
            InvalidHeading,
            Latitude,
            Longitude
        }

        const double Epsilon = 5.0;
        public const double DefaultSpeed = 0.0000025;

        double LatitudeSpeed = DefaultSpeed;
        double LongitudeSpeed = DefaultSpeed;

        private ObservableCollection<Location> _wayPoints;

        private Waypoint StartPosition = new Waypoint();

        public double DistanceToPlayer { get; set; }

        CoarseHeading _heading;
        Random _random;

        private double _eyeAngle;
        public double EyeAngle
        {
            get { return _eyeAngle; }
            set
            {
                if (_eyeAngle != value)
                {
                    _eyeAngle = value;
                    NotifyPropertyChanged("EyeAngle");
                    NotifyPropertyChanged("EyeX");
                    NotifyPropertyChanged("EyeY");
                }
            }
        }

        public double EyeX
        {
            get {
                return 4 * Math.Cos(EyeAngle);
            }
        }

        public double EyeY
        {
            get
            {
                return 4 * Math.Sin(EyeAngle);
            }
        }

        public Ghost(GeoCoordinate position)
        {
            Position = position;
            StartPosition.Location = position;
            _heading = CoarseHeading.InvalidHeading;
            _random = new Random();
            DistanceToPlayer = Double.PositiveInfinity;
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

            EyeAngle = Math.Atan2(Position.Latitude - userPosition.Latitude, userPosition.Longitude - Position.Longitude);
            DistanceToPlayer = Position.GetDistanceTo(userPosition);
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
                    GeoCoordinate userLatitude = new GeoCoordinate(userPosition.Latitude, Position.Longitude);
                    GeoCoordinate userLongitude = new GeoCoordinate(Position.Latitude, userPosition.Longitude);
                    if (userLatitude.GetDistanceTo(Position) < Epsilon)
                    {
                        _heading = CoarseHeading.Longitude;
                    }
                    else if (userLongitude.GetDistanceTo(Position) < Epsilon)
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
                case CoarseHeading.Latitude:
                    diff = userPosition.Latitude - Position.Latitude;
                    Position.Latitude += Math.Sign(diff) * LatitudeSpeed;
                    NotifyPropertyChanged("Position");
                    break;
                case CoarseHeading.Longitude:
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

        public void SetSpeed(double speed)
        {
            LatitudeSpeed = speed;
            LongitudeSpeed = speed;
        }
    }
}
