using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SnakeGameWPF
{
    public interface IElement
    {
        SolidColorBrush Colour { get; }
        UIElement UiElement { get; }
        Point Position { get; }
    }
}
