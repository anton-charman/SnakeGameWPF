using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private int _score = 0;
        private int _multiplier = 50;

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
        private readonly int _startingInterval = 400;
        private readonly int _minimumInterval = 100;

        private bool _isRunning = false;
        public double TileSize => MainArea.ActualWidth / _numSquares;
        private double Interval => Math.Max(_startingInterval - _score * _multiplier, _minimumInterval);


        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the window content being rendered.
        /// </summary>
        private void Window_ContentRendered(object sender, EventArgs e)
        { 
            _snake = new Snake(_startLength, _startDirection, _startRow, _startCol, TileSize);
            _apple = new Apple(TileSize);

            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(Interval);
            _dispatchTimer.Tick += Timer_Tick;

            DrawMainArea();
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
                    Tile tile = new Tile(TileSize, counter);
                    
                    Canvas.SetTop(tile.UiElement, row * TileSize);
                    Canvas.SetLeft(tile.UiElement, col * TileSize);
                    MainArea.Children.Add(tile.UiElement);

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
        /// Event handler for user keystrokes.
        /// </summary>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ResetGame();
                StartNewGame();
                return;
            }

            if(_isRunning)
            {
                SnakeDirection prevDir = _snake.Direction;
                _snake.UpdateDirection(e.Key);

                // I don't like this block of code.
                // It draws the snake before the timer ticks.
                // This allows the user to speed up the snake movement speed by changing direction.
                if (_snake.Direction != prevDir)
                {
                    UpdateSnake();
                    DrawSnake();
                    DoCollisionCheck();
                }
            }   
        }

        /// <summary>
        /// Clear the screen of the snake and apple and reset the score.
        /// </summary>
        private void ResetGame()
        {
            _dispatchTimer.Stop();

            foreach (BaseElement part in _snake.SnakeList)
            {
                MainArea.Children.Remove(part.UiElement);
            }

            MainArea.Children.Remove(_apple.UiElement);

            _snake.ResetSnake(_startLength, _startDirection, _startRow, _startCol, TileSize);

            _score = 0;
            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(Interval);

            borderWelcome.Visibility = Visibility.Collapsed;
            borderUserDied.Visibility = Visibility.Collapsed;

            _isRunning = true;
        }

        /// <summary>
        /// Put the snake and apple onto the screen and kick off the game timer.
        /// </summary>
        private void StartNewGame()
        {
            UpdateTitle();
            DrawSnake();
            _apple.UpdateAppleCoord(_numSquares, _snake.GetSnakePartCoords());
            DrawApple();

            _dispatchTimer.Start(); 
        }

        /// <summary>
        /// Update the main window's score and interval textblock controls.
        /// </summary>
        private void UpdateTitle()
        {
            textBlockScore.Text = _score.ToString();
            textBlockInterval.Text = Interval.ToString();
        }

        /// <summary>
        /// Places the snake head and body onto the canvas. 
        /// </summary>
        private void DrawSnake()
        {
            foreach (BaseElement snakeBodyPart in _snake.SnakeList)
            {
                if (!MainArea.Children.Contains(snakeBodyPart.UiElement))
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

            MainArea.Children.Remove(_snake.SnakeList[0].UiElement);
            MainArea.Children.Remove(_snake.SnakeList[^1].UiElement);
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

        /// <summary>
        /// Checks if the snake head hits an apple, the walls or its body.
        /// </summary>
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

        /// <summary>
        /// Expands the snake, moves the apple, increases the score and speeds the game up.
        /// </summary>
        private void EatApple()
        {
            _score++;
            _dispatchTimer.Interval = TimeSpan.FromMilliseconds(Interval);

            _snake.ExpandBody();

            MainArea.Children.Remove(_apple.UiElement);
            _apple.UpdateAppleCoord(_numSquares, _snake.GetSnakePartCoords());
            DrawApple();

            UpdateTitle();
        }

        /// <summary>
        /// Snake-boundary interaction logic.
        /// </summary>
        private bool GetIsBoundary() => 
            _snake.SnakeList[^1].Position.X < 0 || 
            _snake.SnakeList[^1].Position.X >= MainArea.ActualWidth || 
            _snake.SnakeList[^1].Position.Y < 0 || 
            _snake.SnakeList[^1].Position.Y >= MainArea.ActualHeight;

        /// <summary>
        /// Game closure and cleanup logic.
        /// </summary>
        private void EndGame()
        {
            _dispatchTimer.Stop();
            _isRunning = false;
            textBlockFinalScore.Text = _score.ToString();
            borderUserDied.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Enables the window to be dragged by clicking anywhere.
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Closes the game when our custom 'X' button is pressed.
        /// </summary>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}