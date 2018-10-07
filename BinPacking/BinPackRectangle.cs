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
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set {
                _isSelected = value;
                if(value) {
                    Rectangle.Stroke = new SolidColorBrush(Colors.Red);
                    Rectangle.StrokeThickness = 2;
                } else
                    SetDefaultStroke();
            }
        }
        public bool IsHovered
        {
            get { return IsHovered; }
            set {
                if (value) { 
                    if (!_isSelected) {
                        Rectangle.Stroke = new SolidColorBrush(Colors.Gray);
                        Rectangle.StrokeThickness = 2;
                    }
                } else if(!_isSelected)
                    SetDefaultStroke();
            }
        }

        public BinPackRectangle(Rectangle Rectangle) { this.Rectangle = Rectangle; IsSelected = false; IsAssigned = false; IsHovered = false; }

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

        public bool IsIn(int x, int y, int threshold) => x + threshold >= XPos && y + threshold >= YPos && x - threshold <= XPos + Rectangle.Width && y -threshold <= YPos + Rectangle.Height;

        private void SetDefaultStroke() {
            Rectangle.Stroke = new SolidColorBrush(Colors.Black);
            Rectangle.StrokeThickness = 1;
        }
    }
}
