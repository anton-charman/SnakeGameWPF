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
        // Grid parameters.
        private readonly int _numSquares = 20;
        private double _tileSize;
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        // Snake parameters.
        private readonly int _startRow = 10;
        private readonly int _startCol = 10;
        private readonly int _startLength = 2;
        private readonly SnakeDirection _startingDirection = SnakeDirection.Right;
        private Snake _snake;

        // Apple parameters.
        private Apple _apple;

        // Dispatch timer parameters.
        private readonly DispatcherTimer _dispatchTimer = new DispatcherTimer();
        private readonly int _startingInterval = 1000;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DoInitialisations();
            DrawMainArea();
            DrawSnake();
            DrawApple();
            _dispatchTimer.Start(); // Kicks off the game.
        }

        /// <summary>
        /// Initialises the snake, apple and dispatch timer.
        /// </summary>
        private void DoInitialisations()
        {
            _tileSize = MainArea.ActualWidth / _numSquares;
            _snake = new Snake(_startLength, _startingDirection, _startRow, _startCol, _tileSize);

            _apple = new Apple(_tileSize, _numSquares);

            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(_startingInterval);
            _dispatchTimer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Fill in the game grid in a double for loop.
        /// </summary>
        private void DrawMainArea()
        {
            int counter = 0;
            for (int row = 0; row < _numSquares; row++)
            {
                for (int col = 0; col < _numSquares; col++)
                {
                    Rectangle rect = new Rectangle()
                    {
                        Width = _tileSize,
                        Height = _tileSize,
                        Fill = counter % 2 == 0 ? _evenColour : _oddColour
                    };

                    Canvas.SetTop(rect, row * _tileSize);
                    Canvas.SetLeft(rect, col * _tileSize);
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

        /// <summary>
        /// Places the snake head and body onto the canvas. 
        /// </summary>
        private void DrawSnake()
        {
            foreach (SnakeBodyPart snakeBodyPart in _snake.SnakeList)
            {
                if (snakeBodyPart.UiElement == null)
                {
                    snakeBodyPart.UpdateUIElement(_tileSize);
                    
                    Canvas.SetTop(snakeBodyPart.UiElement, snakeBodyPart.Position.Y);
                    Canvas.SetLeft(snakeBodyPart.UiElement, snakeBodyPart.Position.X);
                    MainArea.Children.Add(snakeBodyPart.UiElement);
                }
            }
        }

        /// <summary>
        /// Places the apple on the canvas.
        /// </summary>
        private void DrawApple()
        {
            Canvas.SetTop(_apple.UiElement, _apple.Position.Y);
            Canvas.SetLeft(_apple.UiElement, _apple.Position.X);
            MainArea.Children.Add(_apple.UiElement);
        }

        /// <summary>
        /// Get a new random apple coordinate unoccupied by the snake. 
        /// </summary>
        private void UpdateAppleCoord()
        {
            _apple.UpdatePosition(_numSquares, _tileSize);
            foreach (SnakeBodyPart part in _snake.SnakeList)
            {
                if (part.Position == _apple.Position)
                {
                    UpdateAppleCoord();
                }
            }
        }

        /// <summary>
        /// Event handler for the dispatch timer tick event.
        /// </summary>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateSnake();
            DrawSnake();
            DoCollisionCheck();
        }

        /// <summary>
        /// Updates the list of snake parts from the movement direction. 
        /// </summary>
        private void UpdateSnake()
        {
            // Remove the current tail of the snake. Defined as the first element.
            while (_snake.SnakeList.Count >= _snake.SnakeLength)
            {
                MainArea.Children.Remove(_snake.SnakeList[0].UiElement);
                _snake.SnakeList.RemoveAt(0);
            }

            _snake.SetToBodyParts();
            _snake.GetNewHead(_tileSize);
        }

        private void DoCollisionCheck()
        {

        }
    }
}