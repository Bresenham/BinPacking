using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BinPacking
{
    public class BinPackRectangle
    {
        public int? XPos { get; set; }
        public int? YPos { get; set; }
        public Rectangle Rectangle { get; }
        public bool IsAssigned { get; set; }
        public bool IsSelected
        {
            get { return IsSelected; }
            set
            {
                if(value)
                {
                    Rectangle.Stroke = new SolidColorBrush(Colors.Red);
                    Rectangle.StrokeThickness = 2;
                } else
                {
                    Rectangle.Stroke = new SolidColorBrush(Colors.Black);
                    Rectangle.StrokeThickness = 1;
                }
            }
        }

        public BinPackRectangle(Rectangle Rectangle) { this.Rectangle = Rectangle; IsSelected = false; IsAssigned = false; }

        public int Draw(Canvas canvas)
        {
            if (XPos != null && YPos != null) {
                Canvas.SetLeft(Rectangle, (double)XPos);
                Canvas.SetTop(Rectangle, (double)YPos);
                canvas.Children.Add(Rectangle);

                return Convert.ToInt32(YPos + Rectangle.Height);
            }

            return -1;
        }
    }
}
