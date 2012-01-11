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
using Microsoft.Phone.Controls;
using System.Windows.Data;

namespace RealityPacman
{
    public partial class StartPage : PhoneApplicationPage
    {
        public StartPage()
        {
            InitializeComponent();

            newGameControl.Difficulty = App.Settings.PreferredDifficulty;
            newGameControl.newGameRequested += new NewGameControl.NewGameRequested(newGameRequested);
            newGameControl.difficultyChanged += new NewGameControl.DifficultyChanged(difficultyChanged);

            ScoresPanoramaItem.DataContext = App.ViewModel;
        }

        private void newGameRequested(Game.Difficulty difficulty)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?difficulty=" + (int)difficulty, UriKind.Relative));
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewGamePage.xaml", UriKind.Relative));
        }

        private void myScoresButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void instructionsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        private void SetScoresDifficultyLabel(string difficulty)
        {
            if (ScoresDifficultyLabel != null)
            {
                ScoresDifficultyLabel.Text = difficulty;
            }
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            newGameControl.Difficulty = App.Settings.PreferredDifficulty;
            base.OnNavigatedTo(e);
        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(newGamePanoramItem))
            {
                newGameControl.Animate();
            }
        }

        private void difficultyChanged(Game.Difficulty difficulty)
        {
            App.Settings.PreferredDifficulty = difficulty;

            switch (difficulty)
            {
                case Game.Difficulty.Easy:
                    SetScoresDifficultyLabel("with easy difficulty");
                    SetScoreListBinding("EasySessions");
                    NoScoresLabel.Visibility = (App.ViewModel.EasySessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case Game.Difficulty.Medium:
                    SetScoresDifficultyLabel("with medium difficulty");
                    SetScoreListBinding("MediumSessions");
                    NoScoresLabel.Visibility = (App.ViewModel.MediumSessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case Game.Difficulty.Hard:
                    SetScoresDifficultyLabel("with hard difficulty");
                    SetScoreListBinding("HardSessions");
                    NoScoresLabel.Visibility = (App.ViewModel.HardSessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }
    }

    public class DurationFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, System.Globalization.CultureInfo cultureInfo)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, (int) value);

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