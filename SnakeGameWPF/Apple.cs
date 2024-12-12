using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class Apple : IElement
    {
        private Random _rnd = new Random();

        public SolidColorBrush Colour => Brushes.Red;
        public UIElement UiElement { get; private set; }
        public Point Position { get; private set; }

        public Apple(double squareSize) 
        {
            UpdateUIElement(squareSize);
        }

        public void UpdateUIElement(double squareSize)
        {
            UiElement = new Ellipse()
            {
                Width = squareSize,
                Height = squareSize,
                Fill = Colour
            };
        }

        /// <summary>
        /// Get a new random apple coordinate unoccupied by the snake. 
        /// </summary>
        public void UpdateAppleCoord(int numSquares, List<Point> snakePosList)
        {
            Position = new Point
            (
                _rnd.Next(numSquares) * ((Ellipse)UiElement).Width, 
                _rnd.Next(numSquares) * ((Ellipse)UiElement).Height
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
