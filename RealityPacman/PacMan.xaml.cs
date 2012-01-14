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
    public partial class PacMan : UserControl
    {
        private DispatcherTimer animTimer;
        private int frame = 0;
        private int delta = 1;

        public PacMan()
        {
            InitializeComponent();
#if false // Disable animation, indicator is no longer a sprite
            animTimer = new DispatcherTimer();
            animTimer.Interval = TimeSpan.FromMilliseconds(50);
            animTimer.Tick += new EventHandler(animTimer_Tick);

            animTimer.Start();
#endif
        }

        public void turn(double angle)
        {
            pacmanRotation.Angle = angle * 180 / Math.PI;
        }

        void animTimer_Tick(object sender, EventArgs e)
        {
            // First and last frame of sprite have white background, skip those frames
            frame = frame + delta;
            if (frame == 10)
                delta = -1;
            else if (frame == 1)
                delta = 1;
            Canvas.SetLeft(sprite, -64 * frame);
        }
    }
}
