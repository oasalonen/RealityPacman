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

namespace RealityPacman
{
    public partial class NewGamePage : PhoneApplicationPage
    {
        public NewGamePage()
        {
            InitializeComponent();

            newGameControl.Difficulty = App.Settings.PreferredDifficulty;
            newGameControl.newGameRequested += new NewGameControl.NewGameRequested(newGameRequested);
        }

        private void newGameRequested(Game.Difficulty difficulty)
        {
            App.Settings.PreferredDifficulty = difficulty;
            NavigationService.Navigate(new Uri("/Ui/MainPage.xaml?difficulty=" + (int)difficulty, UriKind.Relative));
        }
    }
}