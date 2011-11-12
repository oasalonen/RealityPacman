using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using RealityPacman.Models;

namespace RealityPacman.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged, IDisposable
    {
        private DatabaseContext sessionDb;

        private ObservableCollection<SessionModel> _sessions;
        public ObservableCollection<SessionModel> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                NotifyPropertyChanged("Sessions");
            }
        }

        private ObservableCollection<SessionModel> _easySessions;
        public ObservableCollection<SessionModel> EasySessions
        {
            get { return _easySessions; }
            set
            {
                _easySessions = value;
                NotifyPropertyChanged("EasySessions");
            }
        }

        private ObservableCollection<SessionModel> _mediumSessions;
        public ObservableCollection<SessionModel> MediumSessions
        {
            get { return _mediumSessions; }
            set
            {
                _mediumSessions = value;
                NotifyPropertyChanged("MediumSessions");
            }
        }

        private ObservableCollection<SessionModel> _hardSessions;
        public ObservableCollection<SessionModel> HardSessions
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
            sessionDb = new DatabaseContext(dbConnectionString);
        }

        public void Dispose()
        {
            sessionDb.Dispose();
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
            Sessions = new ObservableCollection<SessionModel>(sessionQuery);

            var easySessionQuery = from session in sessionDb.Sessions
                                   orderby session.Duration descending
                                   where session.Difficulty == 0
                                   select session;

            EasySessions = new ObservableCollection<SessionModel>(easySessionQuery);

            var mediumSessionQuery = from session in sessionDb.Sessions
                                     orderby session.Duration descending
                                     where session.Difficulty == 1
                                     select session;

            MediumSessions = new ObservableCollection<SessionModel>(mediumSessionQuery);

            var hardSessionQuery = from session in sessionDb.Sessions
                                   orderby session.Duration descending
                                   where session.Difficulty == 2
                                   select session;

            HardSessions = new ObservableCollection<SessionModel>(hardSessionQuery);
        }

        public void AddSession(SessionModel session)
        {
            sessionDb.Sessions.InsertOnSubmit(session);
            sessionDb.SubmitChanges();
            InsertIntoSessions(Sessions, session);

            Game.Difficulty difficulty = (Game.Difficulty)session.Difficulty;
            switch (difficulty)
            {
                case Game.Difficulty.Easy:
                    InsertIntoSessions(EasySessions, session);
                    break;
                case Game.Difficulty.Medium:
                    InsertIntoSessions(MediumSessions, session);
                    break;
                case Game.Difficulty.Hard:
                    InsertIntoSessions(HardSessions, session);
                    break;
            }
        }

        public void InsertIntoSessions(Collection<SessionModel> sessions, SessionModel session)
        {
            int i = 0;
            while (i < sessions.Count)
            {
                if (sessions[i].Duration < session.Duration)
                {
                    break;
                }
                i++;
            }

            sessions.Insert(i, session);
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
