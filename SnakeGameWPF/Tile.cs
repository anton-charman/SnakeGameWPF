using System.Windows.Media;

namespace SnakeGameWPF
{
    public class Tile : BaseElement
    {
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        public bool IsEven { get; set; }
        public override SolidColorBrush Colour => IsEven ? _evenColour : _oddColour;

        public Tile(double squareSize, int counter)
        {
            IsEven = counter % 2 == 0;
            UpdateUIElement(squareSize);
        }
    }
}
