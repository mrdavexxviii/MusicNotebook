using System.Windows;

namespace MusicNotebook;


public class ScreenSizeProvider 
{
    readonly double verticalRatio = 0.8;
    readonly double horizontalRatio = 0.8;
    readonly double taskbarOffset = 30;
    public double AppStartWidth => SystemParameters.PrimaryScreenWidth * horizontalRatio;
    public double AppStartHeight => SystemParameters.PrimaryScreenHeight * verticalRatio;
    public double HorizontalMargin => SystemParameters.PrimaryScreenWidth * (1-horizontalRatio)/2;
    public double VerticalMargin => Math.Max (0, (SystemParameters.PrimaryScreenHeight * (1 - verticalRatio) / 2) - taskbarOffset);
}
