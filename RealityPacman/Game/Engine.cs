﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Device.Location;

namespace RealityPacman.Game
{
    public class Engine
    {
        private DifficultySettings _difficulty;
        public Difficulty Difficulty 
        {
            get { return _difficulty.Level; }
            set
            {
                _difficulty = new DifficultySettings(value);
            }
        }

        public Player Player;

        public List<Ghost> Ghosts;

        DispatcherTimer _gameTimer;
        const int _tickInterval = 250; // Engine ticks every 250 ms
        const double GhostSpawnMaxLatDiff = 0.001;
        const double GhostSpawnMinLatDiff = 0.0005;
        const double GhostSpawnMaxLonDiff = 0.001;
        const double GhostSpawnMinLonDiff = 0.0005;
        Random _random;
        DateTime _startTime;
        DateTime _endTime;

        public delegate void GhostCreated(Ghost ghost);
        public delegate void GameOver();
        public delegate void GhostsMoved();

        public GhostCreated ghostCreated;
        public GameOver gameOver;
        public GhostsMoved ghostsMoved;

        public Engine()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = new TimeSpan(0, 0, 0, 0, _tickInterval);
            _gameTimer.Tick += new EventHandler(_gameTimer_Tick);

            Difficulty = Difficulty.Easy;

            _random = new Random();
            Player = new Player();
            Ghosts = new List<Ghost>();

            _endTime = new DateTime(0);
            _startTime = new DateTime(0);
        }

        public void Start()
        {
            _gameTimer.Start();
            _startTime = DateTime.Now;
            _endTime = new DateTime(0);
        }

        public void Stop()
        {
            _gameTimer.Stop();
            _endTime = DateTime.Now;
        }

        public void _gameTimer_Tick(Object sender, EventArgs e)
        {
            if (Player.Position == null || Player.Position.IsUnknown)
            {
                return;
            }

            GenerateGhosts();

            // Process each ghost
            int i = 0;
            foreach (Ghost g in Ghosts)
            {
                g.Process(Player.Position);
                //System.Diagnostics.Debug.WriteLine("Ghost pos: " + i++ +
                                                   //" lat: " + g.Position.Latitude +
                                                   //" lon: " + g.Position.Longitude);
                System.Diagnostics.Debug.WriteLine("Ghost " + i++ + " distance: " + g.Position.GetDistanceTo(Player.Position));
                // Check for collision
                if (g.Position.GetDistanceTo(Player.Position) < 10)
                {
                    System.Diagnostics.Debug.WriteLine("You were eaten by ghost " + i + "!");
                    if (gameOver != null)
                    {
                        Stop();
                        gameOver();
                    }
                }
            }

            if (ghostsMoved != null)
            {
                ghostsMoved();
            }
        }

        void GenerateGhosts()
        {
            // Checks whether it is necessary to generate additional ghosts, and does so

            // Likelihood of generating additional ghosts is inversely proportional to number of ghosts
            double ghostCount = Ghosts.Count;

            // Likelihood increases with increasing game time duration
            TimeSpan duration = GameDuration();
            double durationMultiplier;
            double totalMinutes = duration.TotalMinutes;
            if (totalMinutes < 1)
            {
                durationMultiplier = 0.01;
            }
            else if (totalMinutes >= 1 && totalMinutes < 3)
            {
                durationMultiplier = 0.1;
            }
            else
            {
                durationMultiplier = 0.5;
            }

            // And some random stuff to the mix
            double likelihood;
            if (ghostCount == 0)
            {
                likelihood = 1.0;
            }
            else
            {
                likelihood = durationMultiplier / ghostCount;
            }

            if (likelihood >= 1.0)
            {
                // Generate new ghost
                AddNewGhost();
            }
            else
            {
                double threshold = _random.NextDouble();
                if (likelihood > threshold)
                {
                    // Generate new ghost
                    AddNewGhost();
                }
            }
        }

        public TimeSpan GameDuration()
        {
            if (_startTime > _endTime)
            {
                return _startTime - DateTime.Now;
            }
            else
            {
                return _endTime - _startTime;
            }
        }

        void AddNewGhost()
        {
            // Randomize location at X meters from player
            GeoCoordinate ghostPosition = new GeoCoordinate();
            ghostPosition.Latitude = CoordinateWithinBounds(Player.Position.Latitude, GhostSpawnMaxLatDiff, GhostSpawnMinLatDiff);
            ghostPosition.Longitude = CoordinateWithinBounds(Player.Position.Longitude, GhostSpawnMaxLonDiff, GhostSpawnMinLonDiff);

            Ghost newGhost = new Ghost(ghostPosition);
            switch (Difficulty)
            {
                case Difficulty.Easy:
                    newGhost.SetSpeed(Ghost.DefaultSpeed);
                    break;
                case Difficulty.Medium:
                    newGhost.SetSpeed(Ghost.DefaultSpeed * 1.5);
                    break;
                case Difficulty.Hard:
                    newGhost.SetSpeed(Ghost.DefaultSpeed * 2.0);
                    break;
            }

            Ghosts.Add(newGhost);

            if (ghostCreated != null)
            {
                ghostCreated(newGhost);
            }
        }

        double CoordinateWithinBounds(double coordinate, double max, double min)
        {
            return coordinate + (max - min * _random.NextDouble()) * Math.Sign(_random.NextDouble() - 0.5);
        }
    }
}
