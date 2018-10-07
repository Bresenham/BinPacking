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
    public class SpaceHandler
    {
        private readonly List<Space> FreeSpace = new List<Space>();

        public SpaceHandler(int Width, int Height) { FreeSpace.Add(new Space(Width, Height)); }

        public (int x, int y) GetFreeSpace(int Width, int Height)
        {
            Space Free = ChooseBestFit(Width, Height);
            if (Free != null) {
                Space NewSpaceOne = new Space(Free.Width - Width, Height, Free.X + Width, Free.Y);
                Space NewSpaceTwo = new Space(Free.Width, Free.Height - Height, Free.X, Free.Y + Height);
                FreeSpace.Add(NewSpaceOne);
                FreeSpace.Add(NewSpaceTwo);
                FreeSpace.Remove(Free);
                return (Free.X, Free.Y);
            }
            else
                return (-1, -1);
        }

        public void DrawSpaces(Canvas canvas) => FreeSpace.ForEach(space => {
            Rectangle rect = space.Rectangle;
            if (!canvas.Children.Contains(rect)) {
                Canvas.SetLeft(rect, space.X);
                Canvas.SetTop(rect, space.Y);
                canvas.Children.Add(rect);
            }
        });

        public bool MarkRespaced()
        {
            if (FreeSpace.Count < 3)
                return false; ;
            FreeSpace.ForEach(space => space.IsHighlighted = false);
            Space smallestSpace = FreeSpace.Where(space => space.Rectangle.Width == FreeSpace.Min(s => s.Rectangle.Width)).FirstOrDefault();
            if (smallestSpace != null)
            {
                smallestSpace.IsHighlighted = true;
                Space adjacentSpace = FreeSpace.Where(space => space.IsAdjacentTo(smallestSpace)).FirstOrDefault();
                adjacentSpace.IsHighlighted = true;
                return true;
            }
            return false;
        }

        public void ReSpace()
        {
            if (FreeSpace.Count < 3)
                return;
            FreeSpace.ForEach(space => space.IsHighlighted = false);
            Space smallestSpace = FreeSpace.Where(space => space.Rectangle.Width == FreeSpace.Min(s => s.Rectangle.Width)).FirstOrDefault();
            if (smallestSpace != null)
            {
                smallestSpace.IsHighlighted = true;
                Space adjacentSpace = FreeSpace.Where(space => space.IsAdjacentTo(smallestSpace)).FirstOrDefault();
                adjacentSpace.IsHighlighted = true;
                int newHeight = adjacentSpace.Height + smallestSpace.Height;
                FreeSpace.Add(new Space(smallestSpace.Width, newHeight, smallestSpace.X, smallestSpace.Y));
                FreeSpace.Add(new Space(adjacentSpace.Width - smallestSpace.Width, adjacentSpace.Height, adjacentSpace.X, adjacentSpace.Y));
                FreeSpace.Remove(smallestSpace);
                FreeSpace.Remove(adjacentSpace);
            }
        }

        private Space ChooseBestFit(int Width, int Height)
        {
            Dictionary<Space, int> dict = new Dictionary<Space, int>();
            FreeSpace.Where(space => space.DoesFit(Width, Height)).ToList().ForEach(space => dict.Add(space, Math.Min(space.Height - Height, space.Width - Width)));
            return dict.Where(kvp => kvp.Value == dict.Min(kv => kv.Value)).FirstOrDefault().Key;
        }
    }

    class Space
    {
        public int Width { get; }
        public int Height { get; }

        public int X { get; }
        public int Y { get; }

        private bool _isHighlighted = false;
        public bool IsHighlighted { get { return _isHighlighted; } set {
                _isHighlighted = value;
                if (value) Rectangle.Stroke = new SolidColorBrush(Colors.Purple);
                else Rectangle.Stroke = new SolidColorBrush(Colors.Red); }
        }

        public Rectangle Rectangle { get; }

        public Space(int Width, int Height) : this(Width, Height, 0, 0) { }
        public Space(int Width, int Height, int X, int Y)
        {
            Rectangle = new Rectangle
            {
                Width = Width,
                Height = Height,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 1
            };

            this.Width = Width;
            this.Height = Height;
            this.X = X;
            this.Y = Y;
        }

        public bool DoesFit(int Width, int Height) => this.Width >= Width && this.Height >= Height;

        public bool IsAdjacentTo(Space that) => X == that.X + that.Width || X + Width == that.X || Y == that.Y + that.Height || Y + Height == that.Y;
    }
}
