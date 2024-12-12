using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class Tile : IElement
    {
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        public bool IsEven { get; set; }
        public SolidColorBrush Colour => IsEven ? _evenColour : _oddColour;

        public UIElement UiElement { get; private set; }
        public Point Position { get; set; }
        
        public Tile(double squareSize, int counter)
        {
            IsEven = counter % 2 == 0;
            UpdateUIElement(squareSize);
        }

        public void UpdateUIElement(double squareSize)
        {
            UiElement = new Rectangle()
            {
                Width = squareSize,
                Height = squareSize,
                Fill = Colour
            };
        }
    }
}
