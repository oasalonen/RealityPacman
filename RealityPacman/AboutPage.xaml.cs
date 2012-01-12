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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Info;

namespace RealityPacman
{
    public partial class AboutPage : PhoneApplicationPage
    {
        private string ContactEmail = "oasalonen@gmail.com";
        private string AppVersion = "0.5.0";

        public AboutPage()
        {
            InitializeComponent();

            EmailContactButton.Content = ContactEmail;
            VersionField.Text = AppVersion;
        }

        private void emailContact_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask email = new EmailComposeTask();
            email.To = ContactEmail;
            email.Subject = "About Ghost Maps";
            email.Body += 
                "\n\nApplication: Ghost Maps v." + AppVersion +
                "\nDevice: " + DeviceStatus.DeviceManufacturer + " " + DeviceStatus.DeviceName +
                "\nFirmware: " + DeviceStatus.DeviceFirmwareVersion +
                "\nHardware: " + DeviceStatus.DeviceHardwareVersion;
            email.Show();
        }
    }
}