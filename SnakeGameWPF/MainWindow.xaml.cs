﻿using System.Runtime.CompilerServices;
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
        private readonly SolidColorBrush _evenColour = Brushes.LightGreen;
        private readonly SolidColorBrush _oddColour = Brushes.DarkGreen;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawMainArea()
        {
            double squareSize = MainArea.ActualWidth / _numSquares;

            // Fill in the grid.
            int counter = 0;
            for(int row = 0; row < _numSquares; row++)
            {
                for (int col = 0; col < _numSquares; col++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = squareSize;
                    rect.Height = squareSize;
                    rect.Fill = counter % 2 == 0 ? _evenColour : _oddColour;

                    Canvas.SetTop(rect, row * squareSize);
                    Canvas.SetLeft(rect, col * squareSize);
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
    }
}