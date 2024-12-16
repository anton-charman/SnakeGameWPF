using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class Tile : BaseElement
    {
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        private bool _isEven; 
        public override SolidColorBrush Colour => _isEven ? _evenColour : _oddColour;
        public override UIElement UiElement { get; } = new Rectangle();

        public Tile(double squareSize, int counter) : base(squareSize)
        {
            _isEven = counter % 2 == 0;
            UpdateUIElement(squareSize);
        }
    }
}
