using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class Tile
    {
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        public UIElement UiElement { get; init; }
        public Point Position { get; set; }

        public Tile(double tileSize, int counter)
        {
            UiElement = new Rectangle()
            {
                Width = tileSize,
                Height = tileSize,
                Fill = counter % 2 == 0 ? _evenColour : _oddColour
            };
        }
    }
}
