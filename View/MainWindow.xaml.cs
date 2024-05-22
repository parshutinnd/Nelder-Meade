using System.Windows;
using OptimizationMethods;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Threading;
using System;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class PlotView
    {
        public static double width;
        public static double height;
        public static double scalefactor = 10;
        public static double x = 0;
        public static double y = 0;

        public static string StringGenerator(Configuration configuration)
        {
            if (configuration.simplex[0].coords.Length != 2)
            {
                return string.Empty;
            }
            string result_string = "";
            for (int i = 0; i < configuration.simplex.Length; i++)
            {
                if (i != 0) result_string += " ";
                var X = configuration.simplex[i].coords[0] * scalefactor - x;
                var Y = configuration.simplex[i].coords[1] * scalefactor - y;
                result_string += (X).ToString() + ",";
                result_string += (Y).ToString();
            }
            {
                int i = 0;
                result_string += " ";
                var X = configuration.simplex[i].coords[0] * scalefactor - x;
                var Y = configuration.simplex[i].coords[1] * scalefactor - y;
                result_string += (X).ToString() + ",";
                result_string += (Y).ToString();
            }
            return result_string;
        }
    }
    public partial class MainWindow : Window
    {
        double? DragBeginX;
        double? DragBeginY;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void RedrawCoords()
        {
            while (canvas.Children.Count > 1)
            {
                canvas.Children.RemoveAt(1);
            }

            double leftX = PlotView.x / PlotView.scalefactor;
            double leftY = PlotView.y / PlotView.scalefactor;
            double rightX = PlotView.width / PlotView.scalefactor + leftX;
            double rightY = PlotView.height / PlotView.scalefactor + leftY;
            
            double diffX = rightX - leftX;
            double diffY = rightY - leftY;

            if (diffX == 0 || diffY == 0) return;
            double initialTickDiff = 1000;
            while (diffY < initialTickDiff * 5) 
            {
                initialTickDiff /= 2;
                if (diffY >= initialTickDiff * 5) break;
                initialTickDiff /= 5;
            }
            for (double xi = leftX - leftX % initialTickDiff; xi <= rightX; xi+=initialTickDiff)
            {
                Line line = new Line()
                {
                    Stroke = Brushes.Gray,
                    X1 = - PlotView.x + xi * PlotView.scalefactor,
                    X2 = - PlotView.x + xi * PlotView.scalefactor,
                    Y1 = 0,
                    Y2 = PlotView.height
                };
                canvas.Children.Add(line);

                TextBlock textBlock = new TextBlock()
                {
                    Text = xi.ToString(),
                    Foreground = new SolidColorBrush(Colors.Red),
                    Background = new SolidColorBrush(Colors.Black)
                };
                Canvas.SetLeft(textBlock, line.X1);
                Canvas.SetTop(textBlock, 0);
                canvas.Children.Add(textBlock);
            }
            for (double yi = leftY - leftY % initialTickDiff; yi <= rightY; yi += initialTickDiff)
            {
                Line line = new Line()
                {
                    Stroke = Brushes.Gray,
                    Y1 = - PlotView.y + yi * PlotView.scalefactor,
                    Y2 = - PlotView.y + yi * PlotView.scalefactor,
                    X1 = 0,
                    X2 = PlotView.width
                };
                canvas.Children.Add(line);

                TextBlock textBlock = new TextBlock()
                {
                    Text = yi.ToString(),
                    Foreground = new SolidColorBrush(Colors.Red),
                    Background = new SolidColorBrush(Colors.Black)
                };
                Canvas.SetTop(textBlock, line.Y1);
                Canvas.SetBottom(textBlock, 0);
                canvas.Children.Add(textBlock);
            }


            Line OX = new Line()
            {
                Stroke = Brushes.Red,
                Y1 = -PlotView.y,
                Y2 = -PlotView.y,
                X1 = 0,
                X2 = PlotView.width
            };
            Line OY = new Line()
            {
                Stroke = Brushes.Red,
                X1 = -PlotView.x,
                X2 = -PlotView.x,
                Y1 = 0,
                Y2 = PlotView.height
            };

            canvas.Children.Add(OX);
            canvas.Children.Add(OY);
            canvas.InvalidateVisual();
        }
        private void DoublePreviewInput(object sender, TextCompositionEventArgs e)
        {
            string permitted = "0123456789+-.";
            e.Handled = !permitted.Contains(e.Text[0]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PlotView.height = e.NewSize.Height;
            PlotView.width = e.NewSize.Width;
            if (e.PreviousSize.Width == 0 && e.PreviousSize.Height == 0) return;
            RedrawCoords();
        }

        private void Canvas_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                DragBeginX = null;
                DragBeginY = null;
                return;
            }
            if (DragBeginX == null || DragBeginY == null)
            {
                DragBeginX = e.GetPosition(this).X;
                DragBeginY = e.GetPosition(this).Y;
                return;
            }
            var currMousePoint = e.GetPosition(this);

            var dX = currMousePoint.X - (double)DragBeginX;
            var dY = currMousePoint.Y - (double)DragBeginY;
            PlotView.x -= dX;
            PlotView.y -= dY;
            DragBeginX = currMousePoint.X;
            DragBeginY = currMousePoint.Y;
            Replot.Command.Execute(Replot.CommandParameter);
            RedrawCoords();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (PlotView.scalefactor + e.Delta / 10.0f < 0) return;
            PlotView.scalefactor += e.Delta / 10.0f;
            Replot.Command.Execute(Replot.CommandParameter);
            RedrawCoords();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
