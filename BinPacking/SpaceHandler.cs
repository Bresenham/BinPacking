using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinPacking
{
    public class SpaceHandler
    {
        private List<Space> FreeSpace = new List<Space>();

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

        private Space ChooseBestFit(int Width, int Height)
        {
            Dictionary<Space, int> dict = new Dictionary<Space, int>();
            FreeSpace.Where(space => space.DoesFit(Width, Height)).ToList().ForEach(space => dict.Add(space, Math.Min(space.Height - Height, space.Width - Width)));

            return dict.Where(kvp => kvp.Value == dict.Min(kv => kv.Value)).First().Key;
        }
    }

    class Space
    {
        public int Width { get; }
        public int Height { get; }

        public int X { get; }
        public int Y { get; }

        public Space(int Width, int Height) : this(Width, Height, 0, 0) { }

        public Space(int Width, int Height, int X, int Y)
        {
            this.Width = Width;
            this.Height = Height;
            this.X = X;
            this.Y = Y;
        }

        public bool DoesFit(int Width, int Height) => this.Width >= Width && this.Height >= Height;
    }
}
