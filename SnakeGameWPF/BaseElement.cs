using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public abstract class BaseElement
    {
        public BaseElement(double squareSize)
        {
            UpdateUIElement(squareSize);
        }

        public abstract SolidColorBrush Colour { get; }

        public abstract UIElement UiElement { get; set; }

        public Point Position { get; set; }

        public void UpdateUIElement(double squareSize)
        {
            UiElement.SetValue(Shape.WidthProperty, squareSize);
            UiElement.SetValue(Shape.HeightProperty, squareSize);
            UiElement.SetValue(Shape.FillProperty, Colour);
        }
    }
}
