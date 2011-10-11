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
    public partial class StartPage : PhoneApplicationPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void StartButton_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int difficulty = 0;
            if (mediumButton.IsChecked.Value)
            {
                difficulty = 1;
            }
            else if (hardButton.IsChecked.Value)
            {
                difficulty = 2;
            }

            NavigationService.Navigate(new Uri("/MainPage.xaml?playerName=" + "" +
                                               "&difficulty=" + difficulty, UriKind.Relative));
        }
    }
}