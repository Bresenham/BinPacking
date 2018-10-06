using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BinPacking
{
    public class Drawer
    {
        private readonly List<BinPackRectangle> Rectangles;

        private readonly Random Rnd = new Random();

        private readonly Canvas SideCanvas;
        private readonly Canvas MainCanvas;

        private readonly SpaceHandler MainCanvasSpace;

        private readonly int START_X_POS_SIDE = 15;
        private readonly int START_Y_POS_SIDE = 15;

        private BinPackRectangle SelectedRectangle = null;

        private int StartX = 0;
        private int StartY = 0;

        public Drawer(List<BinPackRectangle> Rectangles, Canvas SideCanvas, Canvas MainCanvas){
            this.Rectangles = Rectangles;
            this.SideCanvas = SideCanvas;
            this.MainCanvas = MainCanvas;
            MainCanvasSpace = new SpaceHandler(Convert.ToInt32(MainCanvas.Width), Convert.ToInt32(MainCanvas.Height));
            UpdateSideCanvas();
        }

        public bool AssignRectangleToMain()
        {
            if (SelectedRectangle is null)
                return false;
            UpdateSideCanvas();
            (int XPos, int YPos) = MainCanvasSpace.GetFreeSpace(Convert.ToInt32(SelectedRectangle.Rectangle.Width), 
                                                                Convert.ToInt32(SelectedRectangle.Rectangle.Height));
            if(YPos >= 0 && YPos >= 0)
            {
                SelectedRectangle.XPos = XPos;
                SelectedRectangle.YPos = YPos;
                SelectedRectangle.IsAssigned = true;
                SelectedRectangle.IsSelected = false;
                UpdateMainCanvas();
                return true;
            }
            return false;
        }

        public void SelectRandomRectangle()
        {
            if (Rectangles.Where(rect => !rect.IsAssigned).Count() > 0)
            {
                int randomIndex = Rnd.Next(0, Rectangles.Where(rect => !rect.IsAssigned).Count());
                SelectedRectangle = Rectangles.Where(rect => !rect.IsAssigned).ToArray()[randomIndex];
                SelectedRectangle.IsSelected = true;
            }
            else
                SelectedRectangle = null;
        }

        private void UpdateMainCanvas()
        {
            MainCanvas.Children.Clear();
            foreach (BinPackRectangle rectangle in Rectangles.Where(rect => rect.IsAssigned)) {
                SideCanvas.Children.Remove(rectangle.Rectangle);
                rectangle.Draw(MainCanvas);
            }
        }

        private void UpdateSideCanvas()
        {
            SideCanvas.Children.Clear();
            ResetSideCanvasPositions();
            foreach (BinPackRectangle rectangle in Rectangles.Where(rect => !rect.IsAssigned))
            {
                if (StartY + rectangle.Rectangle.Height >= SideCanvas.Height)
                {
                    BinPackRectangle widestRectangle = Rectangles.Where(r => !r.IsAssigned).OrderByDescending(r => r.Rectangle.Width + r.XPos).FirstOrDefault();
                    StartX = Convert.ToInt32(widestRectangle.XPos + widestRectangle.Rectangle.Width) + 5;
                    StartY = START_Y_POS_SIDE;
                }

                if (rectangle.XPos is null && rectangle.YPos is null)
                {
                    rectangle.XPos = StartX;
                    rectangle.YPos = StartY;
                }

                StartY = rectangle.Draw(SideCanvas) + 5;
            }
        }

        private void ResetSideCanvasPositions()
        {
            StartX = START_X_POS_SIDE;
            StartY = START_Y_POS_SIDE;
        }
    }
}
