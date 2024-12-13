using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    public class SnakeBodyPart : BaseElement
    {
        private SolidColorBrush _snakeHeadColour = Brushes.Black;
        private SolidColorBrush _snakeBodyColour = Brushes.Gray;

        public bool IsHead { get; set; }
        public override SolidColorBrush Colour => IsHead ? _snakeHeadColour : _snakeBodyColour;

        public void SetToBodyPart()
        {
            IsHead = false;
            UiElement.SetValue(Shape.FillProperty, _snakeBodyColour);
        }
    }
}
