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
using System.Windows.Threading;

namespace RealityPacman
{
    public partial class GhostControl : UserControl
    {
        private DispatcherTimer animTimer;
        private int frame = 0;
        private int delta = 1;

        public GhostControl()
        {
            InitializeComponent();

            animTimer = new DispatcherTimer();
            animTimer.Interval = TimeSpan.FromMilliseconds(50);
            animTimer.Tick += new EventHandler(animTimer_Tick);

            animTimer.Start();
        }

        void animTimer_Tick(object sender, EventArgs e)
        {
            frame = frame + delta;
            if (frame == 10)
                delta = -1;
            else if (frame == 0)
                delta = 1;
            Canvas.SetLeft(sprite, -64 * frame);
        }
    }
}
