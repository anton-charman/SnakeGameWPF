using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeTail : BaseElement
    {
        public override SolidColorBrush Colour => Brushes.Blue;
        public override UIElement UiElement { get; set; } = new Rectangle();
    }
}
