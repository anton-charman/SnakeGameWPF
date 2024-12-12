using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeBodyPart : IElement
    {
        private SolidColorBrush _snakeHeadColour = Brushes.Black;
        private SolidColorBrush _colour = Brushes.Gray;

        public SolidColorBrush Colour => IsHead ? _snakeHeadColour : _colour; 
        public UIElement UiElement { get; private set; }
        public Point Position { get; set; }
        public bool IsHead { get; set; }

        public void UpdateUIElement(double squareSize)
        {
            UiElement = new Rectangle
            {
                Width = squareSize,
                Height = squareSize,
                Fill = Colour
            };
        }

        public void SetToBodyPart()
        {
            IsHead = false;
            ((Rectangle) UiElement).Fill = _colour;
        }
    }
}
