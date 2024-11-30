using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
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
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;
        private int _score = 0;
        private int _multiplier = 100;

        // Snake parameters.
        private readonly int _startRow = 10;
        private readonly int _startCol = 10;
        private int _startLength = 2;
        private readonly SnakeDirection _startDirection = SnakeDirection.Right;
        private Snake _snake;

        // Apple parameters.
        private Apple _apple;

        // Dispatch timer parameters.
        private readonly DispatcherTimer _dispatchTimer = new DispatcherTimer();
        private readonly int _startingInterval = 1000;
        private readonly int _minimumInterval = 100;

        public double TileSize => MainArea.ActualWidth / _numSquares;
        private double Interval => Math.Max(_startingInterval - _score * _multiplier, _minimumInterval);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        { 
            _snake = new Snake(_startLength, _startDirection, _startRow, _startCol, TileSize);
            _apple = new Apple(TileSize);

            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(Interval);
            _dispatchTimer.Tick += Timer_Tick;

            DrawMainArea();
            UpdateTitle();
            StartNewGame();
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
                        Width = TileSize,
                        Height = TileSize,
                        Fill = counter % 2 == 0 ? _evenColour : _oddColour
                    };

                    Canvas.SetTop(rect, row * TileSize);
                    Canvas.SetLeft(rect, col * TileSize);
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

        private void UpdateTitle()
        {
            Title = $"Snake in WPF: Score = {_score}, Interval = {Interval} ms";
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ResetGame();
                StartNewGame();
                return;
            }

            SnakeDirection prevDir = _snake.Direction;
            _snake.UpdateDirection(e.Key);

            if (_snake.Direction != prevDir)
            {
                UpdateSnake();
                DrawSnake();
            }
        }

        private void ResetGame()
        {
            _dispatchTimer.Stop();

            foreach (SnakeBodyPart part in _snake.SnakeList)
            {
                MainArea.Children.Remove(part.UiElement);
            }

            MainArea.Children.Remove(_apple.UiElement);

            _snake.ResetSnake(_startLength, _startDirection, _startRow, _startCol, TileSize);

            _score = 0;

            UpdateTitle();
        }

        private void StartNewGame()
        {
            DrawSnake();
            _apple.UpdateAppleCoord(_numSquares, _snake.GetSnakePartCoords());
            DrawApple();

            _dispatchTimer.Start(); // Kicks off the game.
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
                    snakeBodyPart.UpdateUIElement(TileSize);
                    
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
            _snake.UpdateHead(TileSize);
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

        private void DoCollisionCheck()
        {
            if (_snake.SnakeList[^1].Position == _apple.Position)
            {
                EatApple();
                return;
            }

            if (GetIsBoundary())
                EndGame();

            if (_snake.GetIsBodyCollision())
                EndGame();
        }

        private void EatApple()
        {
            _score++;

            _snake.ExpandBody();
            UpdateSnake();
            DrawSnake();

            MainArea.Children.Remove(_apple.UiElement);
            _apple.UpdateAppleCoord(_numSquares, _snake.GetSnakePartCoords());
            DrawApple();

            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(Interval);

            UpdateTitle();
        }

        private bool GetIsBoundary() => 
            _snake.SnakeList[^1].Position.X < 0 || 
            _snake.SnakeList[^1].Position.X >= MainArea.ActualWidth || 
            _snake.SnakeList[^1].Position.Y < 0 || 
            _snake.SnakeList[^1].Position.Y >= MainArea.ActualHeight;

        private void EndGame()
        {
            MessageBox.Show("Game over. Play again?");
            ResetGame();
        }
    }
}