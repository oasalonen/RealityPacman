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
using System.Windows.Threading;
using System.Collections.Generic;
using System.Device.Location;

namespace RealityPacman.Game
{
    public class Engine
    {
        // ---------- Constants ----------
        const int _tickInterval = 250; // Engine ticks every 250 ms

        const double GhostSpawnMaxCoordDiff = 0.001;
        const double GhostSpawnMinCoordDiff = 0.0005;

        const double ItemSpawnMaxCoordDiff = 0.001;
        const double ItemSpawnMinCoordDiff = 0.0001;

        const double ItemSpawnLikelihood = 0.02;
        const int MaximumItemCount = 10;

        // ---------- Members ----------
        private DifficultySettings _difficulty;
        public Difficulty Difficulty 
        {
            get { return _difficulty.Level; }
            set
            {
                _difficulty = new DifficultySettings(value);
            }
        }

        public Session Session { get; set; }

        private GeoCoordinate _coordinate;
        public GeoCoordinate Coordinate
        {
            get { return _coordinate; }
            set
            {
                _coordinate = value;
                if (Player != null)
                {
                    Player.Position = value;
                }
                if (Session != null && Session.StartCoordinate.IsUnknown)
                {
                    Session.StartCoordinate = value;
                }
            }
        }

        public Player Player;
        public List<Ghost> Ghosts;
        public List<WorldObject> WorldItems;

        DispatcherTimer _gameTimer;
        Random _random;
        ProximitySensor _proximitySensor;

        public delegate void GhostCreated(Ghost ghost);
        public delegate void GhostsMoved();
        public delegate void GameStarted();
        public delegate void GameOver(Session session);
        public delegate void WorldObjectCreated(WorldObject worldObject);
        public delegate void WorldObjectRemoved(WorldObject worldObject);

        public GhostCreated ghostCreated;
        public GhostsMoved ghostsMoved;
        public GameStarted gameStarted;
        public GameOver gameOver;
        public WorldObjectCreated worldObjectCreated;
        public WorldObjectRemoved worldObjectRemoved;

        // ---------- Methods ----------

        public Engine()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = new TimeSpan(0, 0, 0, 0, _tickInterval);
            _gameTimer.Tick += new EventHandler(_gameTimer_Tick);

            _proximitySensor = new ProximitySensor();
            _random = new Random();
            Difficulty = Difficulty.Easy;
            Player = new Player();
            Ghosts = new List<Ghost>();
            WorldItems = new List<WorldObject>();
        }

        public void Start()
        {
            Ghosts.Clear();

            Session = new Session();
            Session.Difficulty = Difficulty;
            Session.Start();

            _gameTimer.Start();

            if (gameStarted != null)
            {
                gameStarted();
            }
        }

        public void Stop()
        {
            Session.Stop();
            _gameTimer.Stop();
        }

        public void _gameTimer_Tick(Object sender, EventArgs e)
        {
            if (Player.Position == null || Player.Position.IsUnknown)
            {
                return;
            }

            Session.AddDuration(_tickInterval);

            GenerateGhosts();
            // TODO: item generation disabled for first release
            //GenerateItems();

            // Process each ghost
            foreach (Ghost g in Ghosts)
            {
                g.Process(Player.Position);
                // Check for collision
                if (g.CollidesWith(Player))
                {
                    Stop();
                    if (gameOver != null)
                    {
                        gameOver(Session);
                    }
                }
            }

            if (ghostsMoved != null)
            {
                ghostsMoved();
            }

            _proximitySensor.Process(Ghosts, Player);

            // Consume any colliding items
            List<WorldObject> removeList = new List<WorldObject>();
            foreach (WorldObject o in WorldItems)
            {
                if (Player.CollidesWith(o))
                {
                    Player.Consume(o);
                    removeList.Add(o);
                    Session.FruitsConsumed = Player.FruitsConsumed;
                    if (worldObjectRemoved != null)
                    {
                        worldObjectRemoved(o);
                    }
                }
            }
            foreach (WorldObject o in removeList)
            {
                WorldItems.Remove(o);
            }
        }

        void GenerateGhosts()
        {
            // Checks whether it is necessary to generate additional ghosts, and does so

            // Likelihood of generating additional ghosts is inversely proportional to number of ghosts
            double ghostCount = Ghosts.Count;

            // Likelihood increases with increasing game time duration
            TimeSpan duration = Session.Duration;
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
                durationMultiplier = 0.3;
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

        void GenerateItems()
        {
            if (WorldItems.Count >= MaximumItemCount)
            {
                return;
            }

            double likelihood = ItemSpawnLikelihood;
            if (WorldItems.Count != 0)
            {
                likelihood /= WorldItems.Count;
            }
            double threshold = _random.NextDouble();
            if (likelihood > threshold)
            {
                AddNewItem();
            }
        }

        void AddNewGhost()
        {
            // Randomize location at X meters from player
            GeoCoordinate ghostPosition = new GeoCoordinate();
            ghostPosition.Latitude = CoordinateWithinBounds(Player.Position.Latitude, GhostSpawnMaxCoordDiff, GhostSpawnMinCoordDiff);
            ghostPosition.Longitude = CoordinateWithinBounds(Player.Position.Longitude, GhostSpawnMaxCoordDiff, GhostSpawnMinCoordDiff);

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

        void AddNewItem()
        {
            // Randomize location at X meters from player
            GeoCoordinate position = new GeoCoordinate();
            position.Latitude = CoordinateWithinBounds(Player.Position.Latitude, ItemSpawnMaxCoordDiff, ItemSpawnMinCoordDiff);
            position.Longitude = CoordinateWithinBounds(Player.Position.Longitude, ItemSpawnMaxCoordDiff, ItemSpawnMinCoordDiff);

            Fruit fruit = new Fruit(position);
            WorldItems.Add(fruit);

            if (worldObjectCreated != null)
            {
                worldObjectCreated(fruit);
            }
        }

        double CoordinateWithinBounds(double coordinate, double max, double min)
        {
            return coordinate + (max - min * _random.NextDouble()) * Math.Sign(_random.NextDouble() - 0.5);
        }
    }
}
