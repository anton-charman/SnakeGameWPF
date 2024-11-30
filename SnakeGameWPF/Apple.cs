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
    public class Apple
    {
        private Random _rnd = new Random();
        private SolidColorBrush _colour = Brushes.Red;

        public UIElement UiElement { get; init; }
        public Point Position { get; private set; }

        public Apple(double squareSize, int numSquares) 
        {
            UiElement = new Ellipse()
            {
                Width = squareSize,
                Height = squareSize,
                Fill = _colour
            };
        }

        private void UpdatePosition(int numSquares, double squareSize)
        {
            Position = new Point(_rnd.Next(numSquares) * squareSize, _rnd.Next(numSquares) * squareSize);
        }

        /// <summary>
        /// Get a new random apple coordinate unoccupied by the snake. 
        /// </summary>
        public void UpdateAppleCoord(int numSquares, double squareSize, List<SnakeBodyPart> list)
        {
            UpdatePosition(numSquares, squareSize);
            foreach (SnakeBodyPart part in list)
            {
                if (part.Position == Position)
                {
                    UpdateAppleCoord(numSquares, squareSize, list);
                }
            }
        }
    }
}
