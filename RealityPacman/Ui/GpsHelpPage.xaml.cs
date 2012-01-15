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
    public partial class GpsHelpPage : PhoneApplicationPage
    {
        public GpsHelpPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString["helpTopic"] == "disabledPositioning")
            {
                HelpTitle.Text = "Disabled location service";
                HelpContents.Text = "The location service on this device has been disabled. " + 
                    "This application requires a valid location and you must enable the location " +
                    "service in order to play the game." + 
                    "\n\nTo enable the location service, go to the phone settings and under the location " +
                    "setting switch 'Location services' On.";
            }
            else if (NavigationContext.QueryString["helpTopic"] == "noPosition")
            {
                HelpTitle.Text = "No available location";
                HelpContents.Text = "This game cannot be played without a valid location and the device has " +
                    "not yet received a GPS position. Usually a GPS position can be obtained within one " +
                    "minute, but it can sometimes take up to 12 minutes." +
                    "\n\nIn order to get a location more quickly, make sure that you are outside and that you " +
                    "have a clear view of the sky. Also, make sure that your device has a SIM card with a working " +
                    "data plan. If you are roaming, make sure you have allowed data usage while roaming in the phone settings.";
            }
        }
    }
}