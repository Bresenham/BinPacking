using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BinPacking
{
    public class PackRectangle
    {
        public double Width
        {
            get => Rectangle.Width;
            set => Rectangle.Width = value;
        }

        public double Height
        {
            get => Rectangle.Height;
            set => Rectangle.Height = value;
        }

        public Brush Stroke
        {
            get => Rectangle.Stroke;
            set => Rectangle.Stroke = value;
        }

        public double StrokeThickness
        {
            get => Rectangle.StrokeThickness;
            set => Rectangle.StrokeThickness = value;
        }

        public Brush Fill
        {
            get => Rectangle.Fill;
            set => Rectangle.Fill = value;
        }

        public bool Focusable
        {
            get => Rectangle.Focusable;
            set => Rectangle.Focusable = value;
        }

        public Canvas Canvas { get; set; }

        public bool WasAlignedBefore { get; set; }

        public Rectangle Rectangle { get; }

        public List<PackRectangle> OtherRectangles { get; set; }

        public PackRectangle()
        {
            Rectangle = new Rectangle();

            Rectangle.MouseEnter += (s, e) =>
            {
                Rectangle.Focus();
                if (Stroke != Brushes.Red)
                    Stroke = Brushes.Black;
            };

            Rectangle.MouseLeave += (s, e) =>
            {
                if (Stroke != Brushes.Red)
                    Stroke = Brushes.Gray;
            };

            Rectangle.MouseLeftButtonDown += (s, e) =>
            {
                Console.WriteLine("START DRAG");
                if (Stroke == Brushes.Red)
                    Stroke = Brushes.Gray;
                else
                    Stroke = Brushes.Red;
            };

            Rectangle.MouseMove += (s, e) =>
            {
                if (Stroke == Brushes.Red)
                {
                    Point p = e.GetPosition(Canvas);
                    Point newPos = new Point
                    {
                        X = Convert.ToInt32(p.X - Width / 2),
                        Y = Convert.ToInt32(p.Y - Height / 2)
                    };
                    if (newPos.IsInCanvas(Rectangle, Canvas))
                    {
                        Console.WriteLine($"DRAGGIN TO {newPos.X}|{newPos.Y}.");
                        Canvas.SetLeft(Rectangle, newPos.X);
                        Canvas.SetTop(Rectangle, newPos.Y);
                        Rectangle.AlignToOthers(GetRectangles(), Canvas);
                    }
                    else
                    {
                        Console.WriteLine($"POINT ({newPos.X}|{newPos.Y}) NOT IN CANVAS.");
                        Stroke = Brushes.Gray;
                    }
                }
            };

            Rectangle.MouseLeftButtonUp += (s, e) =>
            {
                Console.WriteLine("STOP DRAG");
                Stroke = Brushes.Gray;
            };

            Rectangle.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Space)
                {
                    double width = Width;
                    Width = Height;
                    Height = width;
                }
            };
        }

        public void SetLeft(int x) => Canvas.SetLeft(Rectangle, x);
        public void SetTop(int y) => Canvas.SetTop(Rectangle, y);

        private List<Rectangle> GetRectangles() => (from pRect in OtherRectangles select pRect.Rectangle).ToList();
    }
}
