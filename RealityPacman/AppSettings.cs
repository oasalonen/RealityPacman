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
using System.IO.IsolatedStorage;

namespace RealityPacman
{
    public class AppSettings
    {
        private IsolatedStorageSettings _settings;
        private const string PreferredDifficultySettingName = "PreferredDifficultySetting";

        public AppSettings()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public Game.Difficulty PreferredDifficulty
        {
            get
            {
                return GetValue<Game.Difficulty>(PreferredDifficultySettingName, Game.Difficulty.Easy);
            }
            set
            {
                SetValue(PreferredDifficultySettingName, value);
            }
        }

        public void SetValue(string key, Object value)
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

        public T GetValue<T>(string key, T defaultValue)
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
    }
}