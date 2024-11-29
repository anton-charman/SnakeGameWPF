using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeBodyPart
    {
        private SolidColorBrush _snakeHeadColour = Brushes.Black;
        private SolidColorBrush _snakeBodyPartColour = Brushes.Gray;

        public UIElement UiElement { get; private set; }
        public Point Position { get; set; }
        public bool IsHead { get; set; }

        public void UpdateUIElement(double squareSize)
        {
            UiElement = new Rectangle
            {
                Width = squareSize,
                Height = squareSize,
                Fill = IsHead ? _snakeHeadColour : _snakeBodyPartColour
            };
        }

        public void SetToBodyPart()
        {
            IsHead = false;
            ((Rectangle) UiElement).Fill = _snakeBodyPartColour;
        }
    }
}
