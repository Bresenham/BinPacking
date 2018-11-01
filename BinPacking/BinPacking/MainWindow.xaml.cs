using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinPacking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly List<(int Width, int Height, int X, int Y)> tupleList = new List<(int Width, int Height, int X, int Y)>();
        private readonly List<PackRectangle> rectangles = new List<PackRectangle>();
        private readonly Random random = new Random();
        private readonly int startX;
        private readonly int startY = 10;
        private readonly int stopX;

        public MainWindow()
        {
            InitializeComponent();
            stopX = Convert.ToInt32(canvas.Width / 2);
            startX = Convert.ToInt32(canvas.Width / 2) + 5;
            CreateRectangles();
            AddRectangles(tupleList);
        }

        private void CreateRectangles()
        {
            int nextX = startX;
            int nextY = startY;
            for (int amount = 0; amount < 5; amount++)
            {
                int rectWidth = random.Next(10, Convert.ToInt32(canvas.Height) / 4);
                int rectHeight = random.Next(10, Convert.ToInt32(canvas.Height) / 4);

                if(nextY + rectHeight > canvas.Height)
                {
                    nextY = startY;
                    nextX += Convert.ToInt32(tupleList.MaxWidth()) + 15;
                }

                tupleList.Add((rectWidth, rectHeight, nextX, nextY));

                nextY += rectHeight + 15;
            }
        }

        private void AddRectangles(List<(int Width, int Height, int X, int Y)> tuples) => tuples.ForEach(tuple => AppendRectangle(tuple));

        private async void AppendRectangle((int Width, int Height, int X, int Y) tuple)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            await Task.Factory.StartNew(() =>
            {
            }).ContinueWith(r =>
            {
                var rectangle = new PackRectangle
                {
                    Width = tuple.Width,
                    Height = tuple.Height,
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                    Focusable = true
                };

                rectangle.Canvas = canvas;
                rectangle.SetLeft(tuple.X);
                rectangle.SetTop(tuple.Y);
                rectangle.OtherRectangles = rectangles;
                canvas.Children.Add(rectangle.Rectangle);
                rectangles.Add(rectangle);

            }, scheduler);
        }
    }
}
