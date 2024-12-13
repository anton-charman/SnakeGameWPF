using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeBodyPart : BaseElement
    {
        public override SolidColorBrush Colour => Brushes.Gray;
        public override UIElement UiElement { get; set; } = new Rectangle();
    }
}
