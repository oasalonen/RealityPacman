using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using RealityPacman.Models;

namespace RealityPacman.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        private SessionDataContext sessionDb;

        private ObservableCollection<Session> _sessions;
        public ObservableCollection<Session> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                NotifyPropertyChanged("Sessions");
            }
        }

        private ObservableCollection<Session> _easySessions;
        public ObservableCollection<Session> EasySessions
        {
            get { return _easySessions; }
            set
            {
                _easySessions = value;
                NotifyPropertyChanged("EasySessions");
            }
        }

        private ObservableCollection<Session> _mediumSessions;
        public ObservableCollection<Session> MediumSessions
        {
            get { return _mediumSessions; }
            set
            {
                _mediumSessions = value;
                NotifyPropertyChanged("MediumSessions");
            }
        }

        private ObservableCollection<Session> _hardSessions;
        public ObservableCollection<Session> HardSessions
        {
            get { return _hardSessions; }
            set
            {
                _hardSessions = value;
                NotifyPropertyChanged("HardSessions");
            }
        }

        public SessionViewModel(string dbConnectionString)
        {
            sessionDb = new SessionDataContext(dbConnectionString);
        }

        public void Save()
        {
            sessionDb.SubmitChanges();
        }

        public void Load()
        {
            var sessionQuery = from session in sessionDb.Sessions
                               orderby session.Duration descending
                               select session;
            Sessions = new ObservableCollection<Session>(sessionQuery);

            var easySessionQuery = from session in sessionDb.Sessions
                                   orderby session.Duration descending
                                   where session.Difficulty == 0
                                   select session;

            EasySessions = new ObservableCollection<Session>(easySessionQuery);

            var mediumSessionQuery = from session in sessionDb.Sessions
                                     orderby session.Duration descending
                                     where session.Difficulty == 1
                                     select session;

            MediumSessions = new ObservableCollection<Session>(mediumSessionQuery);

            var hardSessionQuery = from session in sessionDb.Sessions
                                   orderby session.Duration descending
                                   where session.Difficulty == 2
                                   select session;

            HardSessions = new ObservableCollection<Session>(hardSessionQuery);
        }

        public void AddSession(Session session)
        {
            sessionDb.Sessions.InsertOnSubmit(session);
            sessionDb.SubmitChanges();
            Sessions.Add(session);

            // TODO: add to correct difficulty sessions
        }

        #region INotifyProperyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
