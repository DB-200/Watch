using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Watch;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        CompositionTarget.Rendering += OnRendering;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        BuildTicks(BezelTicks, 240, 220, 4, 2, Brushes.SlateGray, Brushes.DimGray);
        BuildTicks(DialTicks, 185, 160, 3, 1.5, Brushes.Gainsboro, Brushes.DimGray);
        UpdateHands(DateTime.Now);
    }

    private void OnRendering(object? sender, EventArgs e)
    {
        UpdateHands(DateTime.Now);
    }

    private void UpdateHands(DateTime now)
    {
        double seconds = now.Second + (now.Millisecond / 1000.0);
        double minutes = now.Minute + (seconds / 60.0);
        double hours = (now.Hour % 12) + (minutes / 60.0);

        SecondHandRotate.Angle = seconds * 6.0;
        MinuteHandRotate.Angle = minutes * 6.0;
        HourHandRotate.Angle = hours * 30.0;

        DateText.Text = now.ToString("dd");
    }

    private static void BuildTicks(
        Canvas target,
        double outerRadius,
        double innerRadius,
        double majorThickness,
        double minorThickness,
        Brush majorBrush,
        Brush minorBrush)
    {
        target.Children.Clear();

        const double center = 300.0;

        for (int i = 0; i < 60; i++)
        {
            bool isMajor = i % 5 == 0;
            double angle = i * 6.0;
            double radians = angle * Math.PI / 180.0;

            double startRadius = outerRadius;
            double endRadius = isMajor ? innerRadius : innerRadius + 8.0;

            double x1 = center + Math.Sin(radians) * startRadius;
            double y1 = center - Math.Cos(radians) * startRadius;
            double x2 = center + Math.Sin(radians) * endRadius;
            double y2 = center - Math.Cos(radians) * endRadius;

            var tick = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = isMajor ? majorBrush : minorBrush,
                StrokeThickness = isMajor ? majorThickness : minorThickness,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            target.Children.Add(tick);
        }
    }
}
