using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BinPacking
{
    public static class ExtensionMethods
    {
        public static Rectangle WidestRectangle(this List<Rectangle> list) => list.OrderByDescending(x => x.Width).First();

        public static int MaxWidth(this List<(int Width, int Height, int X, int Y)> tuples) => tuples.OrderByDescending(tupl => tupl.Width).First().Width;

        public static bool IsInCanvas(this Rectangle rectangle, Canvas canvas)
        {
            Point topRight = rectangle.TranslatePoint(new Point(rectangle.Width, 0), canvas);
            return topRight.X - rectangle.Width >= 0 && topRight.X  <= canvas.Width && topRight.Y + rectangle.Height <= canvas.Height && topRight.Y >= 0;
        }

        public static bool IsInCanvas(this Point point, Rectangle rectangle, Canvas canvas)
        {
            return point.X >= 0 && point.X + rectangle.Width <= canvas.Width && point.Y >= 0 && point.Y + rectangle.Height <= canvas.Height;
        }

        public static Point TopRight(this Rectangle rectangle, Canvas canvas) => rectangle.GetRectanglePoint(new Point(rectangle.Width, 0), canvas);

        public static Point TopLeft(this Rectangle rectangle, Canvas canvas) => rectangle.GetRectanglePoint(new Point(0, 0), canvas);

        public static Point LowerLeft(this Rectangle rectangle, Canvas canvas) => rectangle.GetRectanglePoint(new Point(0, rectangle.Height), canvas);

        public static Point LowerRight(this Rectangle rectangle, Canvas canvas) => rectangle.GetRectanglePoint(new Point(rectangle.Width, rectangle.Height), canvas);

        private static Point GetRectanglePoint(this Rectangle rectangle, Point point, Canvas canvas) => rectangle.TranslatePoint(point, canvas);

        private static bool EqualsByThreshold(this Point ths, Point that, int thrs)
        {
            return (Math.Abs(ths.X - that.X) <= thrs || ths.X == that.X) && (Math.Abs(ths.Y - that.Y) <= thrs || ths.Y == that.Y);
        }

        public static bool AlignToOthers(this Rectangle rectangle, List<Rectangle> otherRectangles, Canvas canvas)
        {
            int thresh = 5;
            foreach(Rectangle rect in otherRectangles)
            {
                if(rect != rectangle)
                {
                    if (rectangle.TopRight(canvas).EqualsByThreshold(rect.TopLeft(canvas), thresh))
                    {
                        Canvas.SetLeft(rectangle, rect.TopLeft(canvas).X - rectangle.Width);
                        Canvas.SetTop(rectangle, rect.TopLeft(canvas).Y);
                        return true;
                    }
                    else if (rectangle.TopLeft(canvas).EqualsByThreshold(rect.TopRight(canvas), thresh))
                    {
                        Canvas.SetLeft(rectangle, rect.TopRight(canvas).X);
                        Canvas.SetTop(rectangle, rect.TopRight(canvas).Y);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
