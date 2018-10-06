using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private List<BinPackRectangle> rectangles = new List<BinPackRectangle>();
        private readonly Random rnd = new Random();
        private readonly Drawer Drawer;
        private bool selectedRectangle = false;

        public MainWindow()
        {
            InitializeComponent();
            CreateRectangles();
            Drawer = new Drawer(rectangles, sideCanvas, mainCanvas);
        }

        private void CreateRectangles()
        {
            rectangles.Clear();
            for(int i = 0; i < 10; i++)
                rectangles.Add(new BinPackRectangle(new Rectangle {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    Height = rnd.Next(10, 50),
                    Width = rnd.Next(10, 50)
                }));
        }

        private void BtnNextStep_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRectangle) {
                Drawer.AssignRectangleToMain();
                selectedRectangle = false;
            } else {
                Drawer.SelectRandomRectangle();
                selectedRectangle = true;
            }
        }
    }
}
