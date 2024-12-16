using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class Apple : BaseElement 
    {
        public Apple(double squareSize) : base(squareSize) { }

        private Random _rnd = new Random();

        public override SolidColorBrush Colour => Brushes.Red;
        public override UIElement UiElement { get; } = new Ellipse();

        /// <summary>
        /// Get a new random apple coordinate unoccupied by the snake. 
        /// </summary>
        public void UpdateAppleCoord(int numSquares, List<Point> snakePosList)
        {
            // Place the apple anywhere apart from on the border.
            Position = new Point
            (
                _rnd.Next(1, numSquares - 1) * (double)UiElement.GetValue(Shape.WidthProperty), 
                _rnd.Next(1, numSquares - 1) * (double)UiElement.GetValue(Shape.HeightProperty)
            );
            
            foreach (Point pos in snakePosList)
            {
                if (pos == Position)
                {
                    UpdateAppleCoord(numSquares, snakePosList);
                }
            }
        }
    }
}
