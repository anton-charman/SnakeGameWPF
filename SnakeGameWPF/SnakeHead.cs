using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeHead : BaseElement
    {
        public SnakeHead(double squareSize) : base(squareSize) { }

        public override SolidColorBrush Colour => Brushes.Black;
        public override UIElement UiElement { get; } = new Ellipse();
    }
}
