using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public abstract class BaseElement
    {
        public Point Position { get; set; }

        public abstract SolidColorBrush Colour { get; }
    
        public virtual UIElement UiElement { get; set; } = new Rectangle();

        public void UpdateUIElement(double squareSize)
        {
            UiElement.SetValue(Shape.WidthProperty, squareSize);
            UiElement.SetValue(Shape.HeightProperty, squareSize);
            UiElement.SetValue(Shape.FillProperty, Colour);
        }
    }
}
