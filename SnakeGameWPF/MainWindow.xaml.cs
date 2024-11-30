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
        private double _tileSize;
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;
        private bool _isGameActive = false;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _tileSize = MainArea.ActualWidth / _numSquares;
            _apple = new Apple(_tileSize);

            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(GetInterval());
            _dispatchTimer.Tick += Timer_Tick;

            DrawMainArea();
            UpdateTitle();
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                StartNewGame();
                return;
            }
                
            SnakeDirection currDir = _snake.Direction;
            switch (e.Key)
            {
                case Key.Up or Key.W:
                    if (currDir != SnakeDirection.Down)
                    {
                        _snake.Direction = SnakeDirection.Up;
                    }
                    break;
                case Key.Down or Key.S:
                    if (currDir != SnakeDirection.Up)
                    {
                        _snake.Direction = SnakeDirection.Down;
                    }
                    break;
                case Key.Left or Key.A:
                    if (currDir != SnakeDirection.Right)
                    {
                        _snake.Direction = SnakeDirection.Left;
                    }
                    break;
                case Key.Right or Key.D:
                    if (currDir != SnakeDirection.Left)
                    {
                        _snake.Direction = SnakeDirection.Right;
                    }
                    break;
            }

            // This bit makes the snake motion go out of sync with the timer.
            if (_snake.Direction != currDir)
            {
                UpdateSnake();
                DrawSnake();
            }
        }

        private void StartNewGame()
        {
            if (_isGameActive)
                ResetGame();

            DoInitialisations();
            DrawSnake();
            _apple.UpdateAppleCoord(_numSquares, _snake.GetSnakePartCoords());
            DrawApple();

            _dispatchTimer.Start(); // Kicks off the game.
        }

        private void ResetGame()
        {
            _dispatchTimer.Stop();

            foreach (SnakeBodyPart part in _snake.SnakeList)
            {
                MainArea.Children.Remove(part.UiElement);
            }

            MainArea.Children.Remove(_apple.UiElement);

            _score = 0;

            _isGameActive = false;

            UpdateTitle();
        }

        private void DoInitialisations()
        {
            _snake = new Snake(_startLength, _startDirection, _startRow, _startCol, _tileSize);

            _isGameActive = true;
        }

        private double GetInterval() => Math.Max(_startingInterval - _score * _multiplier, _minimumInterval);

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

            // Issue: when the snake eats an apple the head skips the tile where
            // the apple was and jumps ahead to the next tile.
            _snake.ExpandBody();
            UpdateSnake();
            DrawSnake();

            MainArea.Children.Remove(_apple.UiElement);
            _apple.UpdateAppleCoord(_numSquares, _snake.GetSnakePartCoords());
            DrawApple();

            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(GetInterval());

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
            _snake.GetNewHead(_tileSize);
        }

        private void UpdateTitle()
        {
            Title = $"Snake in WPF: Score = {_score}, Interval = {GetInterval()} ms";
        }
    }
}