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

            NavigationService.Navigate(new Uri("/MainPage.xaml?difficulty=" + (int)difficulty, UriKind.Relative));
        }

        private void easyButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScoresDifficultyLabel("(easy)");
            SetScoreListBinding("EasySessions");
        }

        private void mediumButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScoresDifficultyLabel("(medium)");
            SetScoreListBinding("MediumSessions");
        }

        private void hardButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScoresDifficultyLabel("(hard)");
            SetScoreListBinding("HardSessions");
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
    }

    public class DurationFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, System.Globalization.CultureInfo cultureInfo)
        {
            TimeSpan duration = new TimeSpan((Int64) value);

            string durationString = "";
            if (duration.Hours >= 1.0)
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