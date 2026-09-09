using ParetoTool.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ParetoTool;

public partial class ParetoChart : UserControl
{
    private List<ParetoRow> _data = new();
    private readonly List<BarHit> _hits = new();

    private record BarHit(Rect Bounds, ParetoRow Row);

    public ParetoChart()
    {
        InitializeComponent();
    }

    public void SetData(IReadOnlyList<ParetoRow> data)
    {
        _data = data.ToList();
        Redraw();
    }

    private void PlotCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => Redraw();

    private void Redraw()
    {
        PlotCanvas.Children.Clear();
        _hits.Clear();
        TooltipBorder.Visibility = Visibility.Collapsed;

        double w = PlotCanvas.ActualWidth;
        double h = PlotCanvas.ActualHeight;
        if (w < 80 || h < 80)
            return;

        const double left = 64;
        const double right = 56;
        const double top = 36;
        const double bottom = 78;

        double plotW = Math.Max(10, w - left - right);
        double plotH = Math.Max(10, h - top - bottom);

        AddText("Pareto Chart", w / 2, 10, 16, FontWeights.SemiBold, TextAlignment.Center, Brushes.Black);

        if (_data.Count == 0)
        {
            AddText("Enter data and click Generate Chart", w / 2, h / 2, 14, FontWeights.Normal, TextAlignment.Center,
                new SolidColorBrush(Color.FromRgb(107, 114, 128)));
            return;
        }

        double maxValue = _data.Max(r => r.Value);
        if (maxValue <= 0) maxValue = 1;

        var axisBrush = new SolidColorBrush(Color.FromRgb(156, 163, 175));
        var gridBrush = new SolidColorBrush(Color.FromRgb(229, 231, 235));
        var barBrush = (Brush)FindResource("BarBrush");
        var lineBrush = (Brush)FindResource("LineBrush");
        var refBrush = (Brush)FindResource("RefLineBrush");

        // Horizontal grid + left/right ticks
        for (int i = 0; i <= 5; i++)
        {
            double t = i / 5.0;
            double y = top + plotH * (1 - t);
            PlotCanvas.Children.Add(new Line
            {
                X1 = left,
                Y1 = y,
                X2 = left + plotW,
                Y2 = y,
                Stroke = gridBrush,
                StrokeThickness = 1
            });

            AddText(FormatNice(maxValue * t), left - 8, y - 8, 11, FontWeights.Normal, TextAlignment.Right, axisBrush);
            AddText($"{t * 100:0}%", left + plotW + 8, y - 8, 11, FontWeights.Normal, TextAlignment.Left, axisBrush);
        }

        // 80% reference line
        double y80 = top + plotH * (1 - 0.8);
        PlotCanvas.Children.Add(new Line
        {
            X1 = left,
            Y1 = y80,
            X2 = left + plotW,
            Y2 = y80,
            Stroke = refBrush,
            StrokeThickness = 1.25,
            StrokeDashArray = new DoubleCollection { 4, 3 }
        });
        AddText("80%", left + plotW + 8, y80 - 18, 10, FontWeights.SemiBold, TextAlignment.Left, refBrush);

        // Axes
        PlotCanvas.Children.Add(new Line
        {
            X1 = left,
            Y1 = top,
            X2 = left,
            Y2 = top + plotH,
            Stroke = axisBrush,
            StrokeThickness = 1.25
        });
        PlotCanvas.Children.Add(new Line
        {
            X1 = left + plotW,
            Y1 = top,
            X2 = left + plotW,
            Y2 = top + plotH,
            Stroke = axisBrush,
            StrokeThickness = 1.25
        });
        PlotCanvas.Children.Add(new Line
        {
            X1 = left,
            Y1 = top + plotH,
            X2 = left + plotW,
            Y2 = top + plotH,
            Stroke = axisBrush,
            StrokeThickness = 1.25
        });

        int n = _data.Count;
        double slot = plotW / n;
        double barWidth = slot * 0.62;
        var linePoints = new PointCollection();

        for (int i = 0; i < n; i++)
        {
            var row = _data[i];
            double cx = left + slot * (i + 0.5);
            double barH = plotH * (row.Value / maxValue);
            double barX = cx - barWidth / 2;
            double barY = top + plotH - barH;

            var rect = new Rectangle
            {
                Width = barWidth,
                Height = Math.Max(0, barH),
                Fill = barBrush,
                RadiusX = 2,
                RadiusY = 2
            };
            Canvas.SetLeft(rect, barX);
            Canvas.SetTop(rect, barY);
            PlotCanvas.Children.Add(rect);
            _hits.Add(new BarHit(new Rect(barX, barY, barWidth, Math.Max(1, barH)), row));

            double ly = top + plotH * (1 - row.CumulativePercentage / 100.0);
            linePoints.Add(new Point(cx, ly));

            var label = Truncate(row.Category, 14);
            var tb = new TextBlock
            {
                Text = label,
                FontSize = n > 10 ? 10 : 11,
                Foreground = new SolidColorBrush(Color.FromRgb(55, 65, 81)),
                RenderTransformOrigin = new Point(0, 0.5),
                RenderTransform = new RotateTransform(-35)
            };
            Canvas.SetLeft(tb, cx - 4);
            Canvas.SetTop(tb, top + plotH + 8);
            PlotCanvas.Children.Add(tb);
        }

        var polyline = new Polyline
        {
            Points = linePoints,
            Stroke = lineBrush,
            StrokeThickness = 2.25,
            StrokeLineJoin = PenLineJoin.Round
        };
        PlotCanvas.Children.Add(polyline);

        foreach (var p in linePoints)
        {
            var dot = new Ellipse
            {
                Width = 7,
                Height = 7,
                Fill = Brushes.White,
                Stroke = lineBrush,
                StrokeThickness = 1.75
            };
            Canvas.SetLeft(dot, p.X - 3.5);
            Canvas.SetTop(dot, p.Y - 3.5);
            PlotCanvas.Children.Add(dot);
        }

        AddText("Value", 10, top + plotH / 2, 11, FontWeights.SemiBold, TextAlignment.Left, axisBrush, -90);
        AddText("Cumulative %", w - 16, top + plotH / 2, 11, FontWeights.SemiBold, TextAlignment.Left, axisBrush, 90);

        // Legend
        double lx = left;
        double ly2 = h - 18;
        AddLegendSwatch(lx, ly2 - 6, barBrush);
        AddText("Frequency", lx + 18, ly2 - 10, 11, FontWeights.Normal, TextAlignment.Left, Brushes.Black);
        AddLegendLine(lx + 100, ly2 - 2, lineBrush);
        AddText("Cumulative %", lx + 128, ly2 - 10, 11, FontWeights.Normal, TextAlignment.Left, Brushes.Black);
    }

    private void PlotCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        var pos = e.GetPosition(PlotCanvas);
        var hit = _hits.FirstOrDefault(h => h.Bounds.Contains(pos));
        if (hit is null)
        {
            TooltipBorder.Visibility = Visibility.Collapsed;
            return;
        }

        var r = hit.Row;
        TooltipText.Text =
            $"{r.Category}\nValue: {r.Value:0.##}\n% of total: {r.Percentage:0.0}%\nCumulative: {r.CumulativePercentage:0.0}%";
        TooltipBorder.Visibility = Visibility.Visible;

        double tx = pos.X + 14;
        double ty = pos.Y + 14;
        TooltipBorder.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var sz = TooltipBorder.DesiredSize;
        if (tx + sz.Width > PlotCanvas.ActualWidth) tx = pos.X - sz.Width - 8;
        if (ty + sz.Height > PlotCanvas.ActualHeight) ty = pos.Y - sz.Height - 8;
        TooltipBorder.Margin = new Thickness(Math.Max(0, tx), Math.Max(0, ty), 0, 0);
    }

    private void PlotCanvas_MouseLeave(object sender, MouseEventArgs e)
        => TooltipBorder.Visibility = Visibility.Collapsed;

    private void AddText(string text, double x, double y, double size, FontWeight weight,
        TextAlignment align, Brush brush, double angle = 0)
    {
        var tb = new TextBlock
        {
            Text = text,
            FontSize = size,
            FontWeight = weight,
            Foreground = brush
        };

        if (Math.Abs(angle) > 0.01)
        {
            tb.RenderTransformOrigin = new Point(0, 0);
            tb.RenderTransform = new RotateTransform(angle);
        }

        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        double ox = align switch
        {
            TextAlignment.Center => tb.DesiredSize.Width / 2,
            TextAlignment.Right => tb.DesiredSize.Width,
            _ => 0
        };

        Canvas.SetLeft(tb, x - ox);
        Canvas.SetTop(tb, y);
        PlotCanvas.Children.Add(tb);
    }

    private void AddLegendSwatch(double x, double y, Brush brush)
    {
        var r = new Rectangle { Width = 12, Height = 12, Fill = brush, RadiusX = 2, RadiusY = 2 };
        Canvas.SetLeft(r, x);
        Canvas.SetTop(r, y);
        PlotCanvas.Children.Add(r);
    }

    private void AddLegendLine(double x, double y, Brush brush)
    {
        PlotCanvas.Children.Add(new Line
        {
            X1 = x,
            Y1 = y,
            X2 = x + 20,
            Y2 = y,
            Stroke = brush,
            StrokeThickness = 2.25
        });
    }

    private static string Truncate(string s, int max)
        => s.Length <= max ? s : s[..(max - 1)] + "…";

    private static string FormatNice(double v)
    {
        if (v >= 1000) return v.ToString("0.#k", CultureInfo.CurrentCulture).Replace("k", "k");
        if (Math.Abs(v - Math.Round(v)) < 0.05) return v.ToString("0", CultureInfo.CurrentCulture);
        return v.ToString("0.#", CultureInfo.CurrentCulture);
    }
}