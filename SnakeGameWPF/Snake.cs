using System.Windows;
using System.Windows.Input;

namespace SnakeGameWPF
{
    public class Snake
    {
        public int SnakeLength { get; private set; }
        public SnakeDirection Direction { get; private set; }
        public List<BaseElement> SnakeList { get; private set; }
        private Dictionary<Key, SnakeDirection> _dir = new Dictionary<Key, SnakeDirection>()
        {
            {Key.Up , SnakeDirection.Up},
            {Key.Right , SnakeDirection.Right},
            {Key.Down , SnakeDirection.Down},
            {Key.Left , SnakeDirection.Left}
        };

        public Snake(int startLength, SnakeDirection direction, double row, double col, double squareSize)
        {
            SetUpSnake(startLength, direction, row, col, squareSize);
        }

        /// <summary>
        /// Sets up the snake's initial state.
        /// </summary>
        private void SetUpSnake(int startLength, SnakeDirection direction, double row, double col, double squareSize)
        {
            SnakeLength = startLength;
            Direction = direction;
            SnakeList = new List<BaseElement>()
            {
                new SnakeHead() { Position = new Point(row * squareSize, col * squareSize) }
            };
        }

        /// <summary>
        /// Clears and re-initialises the snake instance's parameters.
        /// </summary>
        public void ResetSnake(int startLength, SnakeDirection direction, double row, double col, double squareSize)
        {
            SnakeList.Clear();
            SetUpSnake(startLength, direction, row, col, squareSize);
        }

        /// <summary>
        /// Mark all parts (including the old head) parts as body parts.
        /// </summary>
        public void SetToBodyParts()
        {
            SnakeList[0] = new SnakeTail() { Position = SnakeList[0].Position };
            SnakeList[^1] = new SnakeBodyPart() { Position = SnakeList[^1].Position };
        }
        
        /// <summary>
        /// Update the head position based on the current snake direction.
        /// </summary>
        public void UpdateHead(double squareSize)
        {
            Point newPoint = SnakeList[SnakeList.Count - 1].Position;

            switch (Direction)
            {
                case SnakeDirection.Left:
                    newPoint.X -= squareSize;
                    break;
                case SnakeDirection.Right:
                    newPoint.X += squareSize;
                    break;
                case SnakeDirection.Up:
                    newPoint.Y -= squareSize;
                    break;
                case SnakeDirection.Down:
                    newPoint.Y += squareSize;
                    break;
            }

            // Add the new head. Defined as the last element.
            SnakeList.Add(new SnakeHead() { Position = newPoint });
        }

        /// <summary>
        /// Gets the new snake direction from user keystrokes.
        /// </summary>
        /// <param name="key">User pressed key.</param>
        public void UpdateDirection(Key key)
        {
            // Don't let the snake do a U-turn and don't update direction if it isn't different.
            if (Direction != (SnakeDirection)(((int)_dir[key] + 2) % 4) && Direction != _dir[key])
                Direction = _dir[key];
        }

        /// <summary>
        /// Returns a list of the positions of the snake body parts.
        /// </summary>
        public List<Point> GetSnakePartCoords()
        {
            List<Point> list = new List<Point>();
            foreach(BaseElement part in SnakeList)
            {
                list.Add(part.Position);
            }
            return list;
        }

        /// <summary>
        /// Checks if the snake head collides with its own body.
        /// </summary>
        public bool GetIsBodyCollision()
        {
            for (int i = 0; i < SnakeList.Count - 1; i++)
            {
                if (SnakeList[^1].Position == SnakeList[i].Position)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Increase the snake length property when an apple is eaten.
        /// </summary>
        public void ExpandBody() => SnakeLength++;
    }
}
