using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SnakeGameWPF
{
    public class Snake
    {
        //private readonly int _speedThreshold = 100;
        public int SnakeLength { get; private set; }
        public SnakeDirection Direction { get; set; }
        public List<SnakeBodyPart> SnakeList { get; init; }

        public Snake(int snakeLength, SnakeDirection direction, double row, double col, double squareSize)
        {
            SnakeLength = snakeLength;
            Direction = direction;
            SnakeList = new List<SnakeBodyPart>() 
            { 
                new SnakeBodyPart() { Position = new Point(row * squareSize, col * squareSize), IsHead = true } 
            };
        }

        /// <summary>
        /// Mark all parts (including the old head) parts as body parts.
        /// </summary>
        public void SetToBodyParts()
        {
            foreach (SnakeBodyPart snakeBodyPart in SnakeList)
            {
                snakeBodyPart.SetToBodyPart();
            }
        }
        
        /// <summary>
        /// Make a new head position based on the current snake direction.
        /// </summary>
        public void GetNewHead(double squareSize)
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
            SnakeList.Add(new SnakeBodyPart()
            {
                Position = newPoint,
                IsHead = true
            });
        }
    }
}
