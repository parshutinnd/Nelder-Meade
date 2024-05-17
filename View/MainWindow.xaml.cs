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
            //PlotView.x += (e.NewSize.Width - e.PreviousSize.Width) / 2;
            //PlotView.y += (e.NewSize.Height - e.PreviousSize.Height) / 2;
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
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (PlotView.scalefactor + e.Delta / 10.0f < 0) return;
            PlotView.scalefactor += e.Delta / 10.0f;
            Replot.Command.Execute(Replot.CommandParameter);
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
