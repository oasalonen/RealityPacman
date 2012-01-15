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

namespace GhostMaps
{
    public partial class HighScoresPage : PhoneApplicationPage
    {
        public HighScoresPage()
        {
            InitializeComponent();

            DifficultyPicker.difficultyChanged += new DifficultyPickerControl.DifficultySelectionChanged(x => HighScoresControl.SetDifficulty(x));

            HighScoresControl.SetDifficultyLabelVisibility(Visibility.Collapsed);
            HighScoresControl.DataContext = App.ViewModel;
            HighScoresControl.SetDifficulty(Game.Difficulty.Easy);
        }
    }
}