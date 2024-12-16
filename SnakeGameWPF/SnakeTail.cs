using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeTail : BaseElement
    {
        public SnakeTail(double squareSize) : base(squareSize) { }

        public override SolidColorBrush Colour => Brushes.Blue;
        public override UIElement UiElement { get; } = new Rectangle();
    }
}
