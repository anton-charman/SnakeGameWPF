using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class Tile : BaseElement
    {
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        public bool IsEven { get; set; }
        public override SolidColorBrush Colour => IsEven ? _evenColour : _oddColour;
        public override UIElement UiElement { get; set; } = new Rectangle();

        public Tile(double squareSize, int counter) : base(squareSize)
        {
            IsEven = counter % 2 == 0;
            UpdateUIElement(squareSize);
        }
    }
}
