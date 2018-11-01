using System.Collections.Generic;
using System.Windows.Shapes;
using BinPacking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestExtensionWidestRectangles()
        {
            List<Rectangle> rectangleList = new List<Rectangle>();
            (int Width, int Height)[] tupleArray = new (int, int)[] { (5, 125), (10, 50), (75, 15) };
            foreach ((int Width, int Height) in tupleArray)
                rectangleList.Add(new Rectangle() { Width = Width, Height = Height });
            int expectedWidth = 75;
            Assert.AreEqual(expectedWidth, rectangleList.WidestRectangle().Width);
        }
    }
}
