using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MusicNotebook
{

    public class ScreenSizeProvider 
    {
        double verticalRatio = 0.8;
        double horizontalRatio = 0.8;
        double taskbarOffset = 30;
        public double AppStartWidth => SystemParameters.PrimaryScreenWidth * horizontalRatio;
        public double AppStartHeight => SystemParameters.PrimaryScreenHeight * verticalRatio;
        public double HorizontalMargin => SystemParameters.PrimaryScreenWidth * (1-horizontalRatio)/2;
        public double VerticalMargin => Math.Max (0, (SystemParameters.PrimaryScreenHeight * (1 - verticalRatio) / 2) - taskbarOffset);
    }
}
