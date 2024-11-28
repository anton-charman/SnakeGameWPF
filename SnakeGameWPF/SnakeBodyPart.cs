using System.Windows;

namespace SnakeGameWPF
{
    public class SnakeBodyPart
    {
        public UIElement UiElement { get; set; }
        public Point Position { get; set; }
        public bool IsHead { get; set; }
    }
}
