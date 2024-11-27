using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeGameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _numSquares = 20;
        private readonly double _squareSize;
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        private SolidColorBrush _snakeHeadColour = Brushes.DarkBlue;
        private SolidColorBrush _snakeBodyPartColour = Brushes.LightBlue;
        private List<SnakeBodyPart> _snakeList = new List<SnakeBodyPart>();

        public enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection _snakeDirection = SnakeDirection.Right;
        private int _snakeLength;

        public MainWindow()
        {
            InitializeComponent();
            _squareSize = MainArea.ActualWidth / _numSquares;
        }

        private void DrawMainArea()
        {
            // Fill in the grid.
            int counter = 0;
            for(int row = 0; row < _numSquares; row++)
            {
                for (int col = 0; col < _numSquares; col++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = _squareSize;
                    rect.Height = _squareSize;
                    rect.Fill = counter % 2 == 0 ? _evenColour : _oddColour;

                    Canvas.SetTop(rect, row * _squareSize);
                    Canvas.SetLeft(rect, col * _squareSize);
                    MainArea.Children.Add(rect);

                    counter++;
                }

                // For correct wrap-around depending on grid dimensions.
                if(_numSquares % 2 == 0)
                {
                    counter++;
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawMainArea();
        }

        private void DrawSnake()
        {
            foreach(SnakeBodyPart snakeBodyPart in _snakeList)
            {
                if(snakeBodyPart.UiElement == null)
                {
                    snakeBodyPart.UiElement = new Rectangle
                    {
                        Width = _squareSize,
                        Height = _squareSize,
                        Fill = snakeBodyPart.IsHead ? _snakeHeadColour : _snakeBodyPartColour
                    };

                    Canvas.SetTop(snakeBodyPart.UiElement, snakeBodyPart.Position.Y);
                    Canvas.SetLeft(snakeBodyPart.UiElement, snakeBodyPart.Position.X);
                    MainArea.Children.Add(snakeBodyPart.UiElement);
                }
            }
        }

        private void DoCollisionCheck()
        {

        }

        private void MoveSnake()
        {
            // Remove the current tail of the snake. Defined as the first element.
            while(_snakeList.Count >= _snakeLength)
            {
                MainArea.Children.Remove(_snakeList[0].UiElement);
                _snakeList.RemoveAt(0);
            }

            // Mark the remaining parts as body parts.
            foreach(SnakeBodyPart snakeBodyPart in _snakeList)
            {
                snakeBodyPart.IsHead = false;
                ((Rectangle)snakeBodyPart.UiElement).Fill = _snakeBodyPartColour;
            }

            // Expand the snake based on the current direction.
            Point newPoint = _snakeList[_snakeList.Count - 1].Position;

            switch (_snakeDirection)
            {
                case SnakeDirection.Left:
                    newPoint.X -= _squareSize;
                    break;
                case SnakeDirection.Right:
                    newPoint.X += _squareSize;
                    break;
                case SnakeDirection.Up:
                    newPoint.Y -= _squareSize;
                    break;
                case SnakeDirection.Down:
                    newPoint.Y += _squareSize;
                    break;
            }

            // Add the new head. Defined as the last element.
            _snakeList.Add(new SnakeBodyPart()
            {
                Position = newPoint,
                IsHead = true
            });

            DrawSnake();

            DoCollisionCheck();
        }
    }
}