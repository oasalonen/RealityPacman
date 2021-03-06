﻿using System;
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

namespace GhostMaps
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
            highScoresControl.SetDifficultyLabelVisibility(Visibility.Visible);
        }

        private void newGameRequested(Game.Difficulty difficulty)
        {
            NavigationService.Navigate(new Uri("/Ui/MainPage.xaml?difficulty=" + (int)difficulty, UriKind.Relative));
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ui/NewGamePage.xaml", UriKind.Relative));
        }

        private void myScoresButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ui/HighScoresPage.xaml", UriKind.Relative));
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ui/SettingsPage.xaml", UriKind.Relative));
        }

        private void instructionsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ui/HelpPage.xaml", UriKind.Relative));
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ui/AboutPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            App.IsIdleRunningEnabled = App.Settings.IsIdleRunningEnabled;
            newGameControl.Difficulty = App.Settings.PreferredDifficulty;
            highScoresControl.SetDifficulty(App.Settings.PreferredDifficulty);

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
            highScoresControl.SetDifficulty(difficulty);
        }
    }
}