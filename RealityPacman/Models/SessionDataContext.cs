using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace RealityPacman.Models
{
    [Table]
    public class Session : INotifyPropertyChanged, INotifyPropertyChanging
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

        private Int64 _duration;
        [Column]
        public Int64 Duration
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

    public class SessionDataContext : DataContext
    {
        public SessionDataContext(string connectionString) : base(connectionString) { }

        public Table<Session> Sessions;
    }
}
