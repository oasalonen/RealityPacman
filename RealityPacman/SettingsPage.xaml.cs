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
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            PermissionSettingsControl.AllowLocationCheckBox.Checked += new RoutedEventHandler((x,y) => App.Settings.IsLocationAccessAllowed = true);
            PermissionSettingsControl.AllowLocationCheckBox.Unchecked += new RoutedEventHandler((x, y) => App.Settings.IsLocationAccessAllowed = false);

            PermissionSettingsControl.AllowRunningIdleCheckBox.Checked += new RoutedEventHandler((x, y) => App.Settings.IsIdleRunningEnabled = true);
            PermissionSettingsControl.AllowRunningIdleCheckBox.Unchecked += new RoutedEventHandler((x, y) => App.Settings.IsIdleRunningEnabled = false);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PermissionSettingsControl.AllowLocationCheckBox.IsChecked = App.Settings.IsLocationAccessAllowed;
            PermissionSettingsControl.AllowRunningIdleCheckBox.IsChecked = App.Settings.IsIdleRunningEnabled;
        }
    }
}