using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeHead : BaseElement
    {
        public override SolidColorBrush Colour => Brushes.Black;
        public override UIElement UiElement { get; set; } = new Ellipse();
    }
}
