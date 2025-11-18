using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace UI
{
    public static class AnimationClearWPF
    {
        // Fade-out từ giữa ra hai bên
        public static void AnimateClearFade(Rectangle[,] rectangles, List<int> fullRows, List<int> fullCols)
        {
            double baseDelay = 100; // ms delay giữa các ô
            double duration = 300;  // thời gian fade mỗi ô

            // Hàng
            foreach (int r in fullRows)
            {
                double middle = (8 - 1) / 2.0;
                for (int c = 0; c < 8; c++)
                {
                    double distance = Math.Abs(c - middle);
                    ApplyFade(rectangles[r, c], distance * baseDelay, duration);
                }
            }

            // Cột
            foreach (int c in fullCols)
            {
                double middle = (8 - 1) / 2.0;
                for (int r = 0; r < 8; r++)
                {
                    double distance = Math.Abs(r - middle);
                    ApplyFade(rectangles[r, c], distance * baseDelay, duration);
                }
            }
        }

        private static void ApplyFade(Rectangle rect, double beginTimeMs, double durationMs)
        {
            DoubleAnimation fade = new DoubleAnimation
            {
                To = 0.0, // fade-out
                Duration = TimeSpan.FromMilliseconds(durationMs),
                BeginTime = TimeSpan.FromMilliseconds(beginTimeMs)
            };

            rect.BeginAnimation(Rectangle.OpacityProperty, fade);
        }
    }
}
//AnimationClearWPF.AnimateClearFade(rectangles, fullRows, fullCols);