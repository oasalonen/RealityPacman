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

            InstructionText.Text = "Escape the ghosts on the map by moving your legs. Yes, you need to go outside to play this game." +
                "\r\n\r\nMind your surroundings while playing. Cars heading your way are not indicated on the map!";

            ScoresPanoramaItem.DataContext = App.ViewModel;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Difficulty difficulty;
            if (easyButton.IsChecked.Value)
            {
                difficulty = Game.Difficulty.Easy;
            }
            else if (mediumButton.IsChecked.Value)
            {
                difficulty = Game.Difficulty.Medium;
            }
            else
            {
                difficulty = Game.Difficulty.Hard;
            }

            App.Settings.PreferredDifficulty = difficulty;
            NavigationService.Navigate(new Uri("/MainPage.xaml?difficulty=" + (int)difficulty, UriKind.Relative));
        }

        private void easyButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScoresDifficultyLabel("with easy difficulty");
            SetScoreListBinding("EasySessions");
            NoScoresLabel.Visibility = (App.ViewModel.EasySessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void mediumButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScoresDifficultyLabel("with medium difficulty");
            SetScoreListBinding("MediumSessions");
            NoScoresLabel.Visibility = (App.ViewModel.MediumSessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void hardButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScoresDifficultyLabel("with hard difficulty");
            SetScoreListBinding("HardSessions");
            NoScoresLabel.Visibility = (App.ViewModel.HardSessions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
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
            switch (App.Settings.PreferredDifficulty)
            {
                case Game.Difficulty.Easy:
                    easyButton.IsChecked = true;
                    break;
                case Game.Difficulty.Medium:
                    mediumButton.IsChecked = true;
                    break;
                case Game.Difficulty.Hard:
                    hardButton.IsChecked = true;
                    break;
            }
            base.OnNavigatedTo(e);
        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(newGamePanoramItem))
            {
                startAnimation.Begin();
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