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

namespace GhostMaps
{
    public partial class HighScoresControl : UserControl
    {
        public HighScoresControl()
        {
            InitializeComponent();
        }

        public void SetDifficulty(Game.Difficulty difficulty)
        {
            string difficultyLabelText = "";

            switch (difficulty)
            {
                case Game.Difficulty.Easy:
                    difficultyLabelText = "with easy difficulty";
                    SetScoreListBinding("EasySessions");
                    NoScoresLabel.Visibility = (App.ViewModel.EasySessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case Game.Difficulty.Medium:
                    difficultyLabelText = "with medium difficulty";
                    SetScoreListBinding("MediumSessions");
                    NoScoresLabel.Visibility = (App.ViewModel.MediumSessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case Game.Difficulty.Hard:
                    difficultyLabelText = "with hard difficulty";
                    SetScoreListBinding("HardSessions");
                    NoScoresLabel.Visibility = (App.ViewModel.HardSessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }

            if (ScoresDifficultyLabel != null)
            {
                ScoresDifficultyLabel.Text = difficultyLabelText;
            }
        }

        public void SetDifficultyLabelVisibility(Visibility visibility)
        {
            ScoresDifficultyLabel.Visibility = visibility;
        }

        private void SetScoreListBinding(string sessions)
        {
            if (ScoresList != null)
            {
                Binding b = new Binding(sessions);
                b.Mode = BindingMode.OneTime;

                ScoresList.SetBinding(ListBox.ItemsSourceProperty, b);
            }
        }
    }

    public class DurationFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, System.Globalization.CultureInfo cultureInfo)
        {
            TimeSpan duration = TimeSpan.FromMilliseconds((long)value);

            string durationString = "";
            if (duration.Days >= 1.0)
            {
                durationString += (int)duration.Days + " d " + (int)duration.Hours + " h " + (int)duration.Minutes + " min ";
            }
            else if (duration.Hours >= 1.0)
            {
                durationString += (int)duration.Hours + " h " + (int)duration.Minutes + " min ";
            }
            else if (duration.Minutes >= 1.0)
            {
                durationString += (int)duration.Minutes + " min ";
            }
            durationString += (int)duration.Seconds + " s";
            return durationString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
