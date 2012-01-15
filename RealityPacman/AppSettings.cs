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
using System.IO.IsolatedStorage;
using System.ComponentModel;

namespace GhostMaps
{
    public class AppSettings : INotifyPropertyChanged
    {
        private IsolatedStorageSettings _settings;
        private const string PreferredDifficultySettingName = "PreferredDifficultySetting";
        private const string IsDisclaimerAcceptedSettingName = "IsDisclaimerAcceptedSetting";
        private const string IsPermissionPageShownSettingName = "IsPermissionPageShownSetting";
        private const string IsIdleRunningEnabledSettingName = "IsIdleRunningEnabledSetting";
        private const string IsLocationAccessAllowedSettingName = "IsLocationAccessAllowedSetting";

        public AppSettings()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        #region Settings
        public Game.Difficulty PreferredDifficulty
        {
            get
            {
                return GetValue<Game.Difficulty>(PreferredDifficultySettingName, Game.Difficulty.Easy);
            }
            set
            {
                SetValue(PreferredDifficultySettingName, value);
                NotifyPropertyChanged("PreferredDifficulty");
            }
        }

        public bool IsDisclaimerAccepted
        {
            get
            {
                return GetValue<bool>(IsDisclaimerAcceptedSettingName, false);
            }
            set
            {
                SetValue(IsDisclaimerAcceptedSettingName, value);
                NotifyPropertyChanged("IsDisclaimerAccepted");
            }
        }

        public bool IsPermissionPageShown
        {
            get
            {
                return GetValue<bool>(IsPermissionPageShownSettingName, false);
            }
            set
            {
                SetValue(IsPermissionPageShownSettingName, value);
                NotifyPropertyChanged("IsPermissionPageShown");
            }
        }

        public bool IsIdleRunningEnabled
        {
            get
            {
                return GetValue<bool>(IsIdleRunningEnabledSettingName, false);
            }
            set
            {
                SetValue(IsIdleRunningEnabledSettingName, value);
                NotifyPropertyChanged("IsIdleRunningEnabled");
            }
        }

        public bool IsLocationAccessAllowed
        {
            get
            {
                return GetValue<bool>(IsLocationAccessAllowedSettingName, true);
            }
            set
            {
                SetValue(IsLocationAccessAllowedSettingName, value);
                NotifyPropertyChanged("IsLocationAccessAllowed");
            }
        }
        #endregion

        #region Private get/set
        private void SetValue(string key, Object value)
        {
            if (_settings.Contains(key))
            {
                _settings[key] = value;
            }
            else
            {
                _settings.Add(key, value);
            }
        }

        private T GetValue<T>(string key, T defaultValue)
        {
            if (_settings.Contains(key))
            {
                return (T)_settings[key];
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region PropertyNotification
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
