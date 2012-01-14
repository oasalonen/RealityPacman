using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;

namespace RealityPacman.Models
{
    [Table]
    public class SessionModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [Column(IsVersion = true)]
        private Binary _version;

        private int _sessionId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int SessionId
        {
            get { return _sessionId; }
            set
            {
                if (_sessionId != value)
                {
                    NotifyPropertyChanging("SessionId");
                    _sessionId = value;
                    NotifyPropertyChanged("SessionId");
                }
            }
        }

        private int _difficulty;
        [Column]
        public int Difficulty
        {
            get { return _difficulty; }
            set
            {
                if (_difficulty != value)
                {
                    NotifyPropertyChanging("Difficulty");
                    _difficulty = value;
                    NotifyPropertyChanged("Difficulty");
                }
            }
        }

        private long _duration;
        [Column]
        public long Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    NotifyPropertyChanging("Duration");
                    _duration = value;
                    NotifyPropertyChanged("Duration");
                }
            }
        }

        private DateTime _startTime;
        [Column]
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    NotifyPropertyChanging("StartTime");
                    _startTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        private DateTime _endTime;
        [Column]
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    NotifyPropertyChanging("EndTime");
                    _endTime = value;
                    NotifyPropertyChanged("EndTime");
                }
            }
        }

        private double _startLatitude;
        [Column]
        public double StartLatitude
        {
            get { return _startLatitude; }
            set
            {
                if (_startLatitude != value)
                {
                    NotifyPropertyChanging("StartLatitude");
                    _startLatitude = value;
                    NotifyPropertyChanged("StartLatitude");
                }
            }
        }

        private double _startLongitude;
        [Column]
        public double StartLongitude
        {
            get { return _startLongitude; }
            set
            {
                if (_startLongitude != value)
                {
                    NotifyPropertyChanging("StartLongitude");
                    _startLongitude = value;
                    NotifyPropertyChanged("StartLongitude");
                }
            }
        }

        private double _startAltitude;
        [Column]
        public double StartAltitude
        {
            get { return _startAltitude; }
            set
            {
                if (_startAltitude != value)
                {
                    NotifyPropertyChanging("StartAltitude");
                    _startAltitude = value;
                    NotifyPropertyChanged("StartAltitude");
                }
            }
        }

        private int _fruitsConsumed;
        [Column]
        public int FruitsConsumed
        {
            get { return _fruitsConsumed; }
            set
            {
                if (_fruitsConsumed != value)
                {
                    NotifyPropertyChanging("FruitsConsumed");
                    _fruitsConsumed = value;
                    NotifyPropertyChanged("FruitsConsumed");
                }
            }
        }

        public SessionModel() { }

        public SessionModel(Game.Session session)
        {
            Difficulty = (int)session.Difficulty;
            Duration = (int)session.Duration.TotalMilliseconds;
            StartTime = session.StartTime;
            EndTime = session.EndTime;
            StartLatitude = session.StartCoordinate.Latitude;
            StartLongitude = session.StartCoordinate.Longitude;
            StartAltitude = session.StartCoordinate.Altitude;
            FruitsConsumed = session.FruitsConsumed;
        }

        #region INotifyPropertyChanged, INotifyPropertyChanging
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class DatabaseContext : DataContext
    {
        public DatabaseContext(string connectionString) : base(connectionString) { }

        public Table<SessionModel> Sessions;

        public void SetDatabaseVersion(int version)
        {
            DatabaseSchemaUpdater updater = this.CreateDatabaseSchemaUpdater();
            updater.DatabaseSchemaVersion = version;
            updater.Execute();
        }

        public void MigrateDatabase(int newVersion)
        {
            /**
             * How to migrate: 
             * 
             * - Create a new column with the CanBeNull attribute.
             * - When using base types such as int, make it nullable.
             * - Increment the DatabaseVersion field in App.xaml.cs.
             * - Enable the code block below and add a new case with the old version
             * - and add the corresponding column. The first one is left as an example.
             */
#if false
            DatabaseSchemaUpdater updater = this.CreateDatabaseSchemaUpdater();
            int oldVersion = updater.DatabaseSchemaVersion;

            while (oldVersion < newVersion)
            {
                switch (oldVersion)
                {
                    case 1:
                        updater.AddColumn<SessionModel>("NAME OF COLUMN HERE");
                        break;
                }
                updater.DatabaseSchemaVersion = ++oldVersion;
                updater.Execute();
            }
#endif
        }
    }
}
