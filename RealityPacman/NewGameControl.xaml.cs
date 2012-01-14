using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace RealityPacman
{
    public partial class NewGameControl : UserControl
    {
        public Game.Difficulty Difficulty
        {
            get
            {
                return DifficultyPicker.Difficulty;
            }
            set
            {
                DifficultyPicker.Difficulty = value;
            }
        }

        public delegate void DifficultyChanged(Game.Difficulty difficulty);
        public delegate void NewGameRequested(Game.Difficulty difficulty);

        public DifficultyChanged difficultyChanged;
        public NewGameRequested newGameRequested;

        public NewGameControl()
        {
            InitializeComponent();

            DifficultyPicker.difficultyChanged += new DifficultyPickerControl.DifficultySelectionChanged(DifficultySelectionChanged);
            this.DataContext = App.Settings;
        }

        public void Animate()
        {
            StartAnimation.Begin();
        }

        private void DifficultySelectionChanged(Game.Difficulty difficulty)
        {
            if (difficultyChanged != null)
            {
                difficultyChanged(difficulty);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartAnimation.Stop();

            if (newGameRequested != null)
            {
                newGameRequested(Difficulty);
            }
        }
    }

    #region Converters
    public class LocationAccessVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isLocationAllowed = (bool)value;
            if (isLocationAllowed)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
