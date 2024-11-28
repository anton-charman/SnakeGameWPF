using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _numSquares = 20;
        private double _squareSize;
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        private SolidColorBrush _snakeHeadColour = Brushes.Black;
        private SolidColorBrush _snakeBodyPartColour = Brushes.Gray;
        private List<SnakeBodyPart> _snakeList = new List<SnakeBodyPart>();

        public enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection _snakeDirection;
        private int _snakeLength;

        private readonly int _startLength = 2;
        private readonly int _startSpeed = 1000;
        private readonly int _speedThreshold = 100;

        private readonly DispatcherTimer _dispatchTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void DrawMainArea()
        {
            // Initialisations.
            _squareSize = MainArea.ActualWidth / _numSquares;
            int counter = 0;

            // Fill in the grid.
            for (int row = 0; row < _numSquares; row++)
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
                if (_numSquares % 2 == 0)
                {
                    counter++;
                }
            }
        }

        private void StartNewGame()
        {
            // Initialise the snake's starting parameters.
            _snakeLength = _startLength;
            _snakeDirection = SnakeDirection.Right;
            double startRow = _squareSize * (int)(_numSquares / 2);
            double startCol = startRow;
            _snakeList.Add(new SnakeBodyPart()
            {
                Position = new Point(startRow, startCol),
                IsHead = true
            });

            DrawSnake();

            // Initialise the dispatch timer.
            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(_startSpeed);
            _dispatchTimer.Tick += Timer_Tick;
            _dispatchTimer.Start();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawMainArea();
            StartNewGame();
        }

        /// <summary>
        /// Places the snake head and body parts onto the canvas. 
        /// </summary>
        private void DrawSnake()
        {
            foreach (SnakeBodyPart snakeBodyPart in _snakeList)
            {
                if (snakeBodyPart.UiElement == null)
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
            while (_snakeList.Count >= _snakeLength)
            {
                MainArea.Children.Remove(_snakeList[0].UiElement);
                _snakeList.RemoveAt(0);
            }

            // Mark all parts (including the old head) parts as body parts.
            foreach (SnakeBodyPart snakeBodyPart in _snakeList)
            {
                snakeBodyPart.IsHead = false;
                ((Rectangle)snakeBodyPart.UiElement).Fill = _snakeBodyPartColour;
            }

            // Make a new head position based on the current snake direction.
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