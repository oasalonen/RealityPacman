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

namespace GhostMaps
{
    public partial class DifficultyPickerControl : UserControl
    {
        public Game.Difficulty Difficulty
        {
            get
            {
                switch (DifficultyPicker.SelectedIndex)
                {
                    case 0:
                        return Game.Difficulty.Easy;
                    case 1:
                        return Game.Difficulty.Medium;
                    case 2:
                        return Game.Difficulty.Hard;
                    default:
                        return Game.Difficulty.Easy;
                }
            }
            set
            {
                switch (value)
                {
                    case Game.Difficulty.Easy:
                        DifficultyPicker.SelectedIndex = 0;
                        break;
                    case Game.Difficulty.Medium:
                        DifficultyPicker.SelectedIndex = 1;
                        break;
                    case Game.Difficulty.Hard:
                        DifficultyPicker.SelectedIndex = 2;
                        break;
                }
            }
        }

        public delegate void DifficultySelectionChanged(Game.Difficulty difficulty);
        public DifficultySelectionChanged difficultyChanged;

        public DifficultyPickerControl()
        {
            InitializeComponent();

            List<string> difficulties = new List<string>();
            difficulties.Add("easy");
            difficulties.Add("medium");
            difficulties.Add("hard");
            DifficultyPicker.DataContext = difficulties;
        }

        private void DifficultyPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (difficultyChanged != null)
            {
                difficultyChanged(Difficulty);
            }
        }
    }
}
