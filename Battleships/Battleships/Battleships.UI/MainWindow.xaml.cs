using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Battleships.UI
{
    public partial class MainWindow : Window
    {
        private const int GridSize = 10;
        private bool[,] placementGrid = new bool[GridSize, GridSize]; // Tracks ship placements
        private bool[,] shootingGrid = new bool[GridSize, GridSize]; // Tracks shots
        private int[] shipLengths = { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
        private Button[,] placementButtons = new Button[GridSize, GridSize];
        private Button[,] shootingButtons = new Button[GridSize, GridSize];
        private int currentShipIndex = 0;
        private bool isHorizontal = true;
        private bool allShipsPlaced = false;

        public MainWindow()
        {
            InitializeComponent();
            CreatePlacementGrid();
            CreateShootingGrid();
            this.KeyDown += MainWindow_KeyDown; // Handle key presses
        }

        private void CreatePlacementGrid()
        {
            PlacementGrid.Children.Clear();
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Button btn = new Button
                    {
                        Tag = new Point(row, col),
                        Content = new Grid(), // Use a Grid to hold custom content
                        Background = Brushes.White,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1)
                    };
                    btn.Click += PlacementButton_Click;
                    btn.MouseEnter += PlacementButton_MouseEnter;
                    btn.MouseLeave += PlacementButton_MouseLeave;
                    PlacementGrid.Children.Add(btn);
                    placementButtons[row, col] = btn;
                }
            }
        }

        private void CreateShootingGrid()
        {
            ShootingGrid.Children.Clear();
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Button btn = new Button
                    {
                        Tag = new Point(row, col),
                        Content = new Grid(), // Use a Grid to hold custom content
                        Background = Brushes.White,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        IsEnabled = false // Disabled until all ships are placed
                    };
                    btn.Click += ShootingButton_Click;
                    ShootingGrid.Children.Add(btn);
                    shootingButtons[row, col] = btn;
                }
            }
        }

        private void PlacementButton_Click(object sender, RoutedEventArgs e)
        {
            if (allShipsPlaced) return; // Ships already placed

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                Point position = (Point)clickedButton.Tag;
                int row = (int)position.X;
                int col = (int)position.Y;

                if (CanPlaceShip(row, col, shipLengths[currentShipIndex], isHorizontal))
                {
                    PlaceShip(row, col, shipLengths[currentShipIndex], isHorizontal);
                    currentShipIndex++;
                    if (currentShipIndex >= shipLengths.Length)
                    {
                        allShipsPlaced = true;
                        LockPlacementGrid();
                        EnableShootingGrid();
                        MessageBox.Show("All ships placed! Ready to shoot.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid placement. Try again.");
                }
            }
        }

        private void PlacementButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (allShipsPlaced) return; // Ships already placed

            Button hoveredButton = sender as Button;
            if (hoveredButton != null)
            {
                Point position = (Point)hoveredButton.Tag;
                int row = (int)position.X;
                int col = (int)position.Y;

                bool isValid = CanPlaceShip(row, col, shipLengths[currentShipIndex], isHorizontal);
                UpdatePreview(row, col, shipLengths[currentShipIndex], isHorizontal, isValid);
            }
        }

        private void PlacementButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ClearPreview();
        }

        private void ShootingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!allShipsPlaced) return; // Ships not placed yet

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                Point position = (Point)clickedButton.Tag;
                int row = (int)position.X;
                int col = (int)position.Y;

                if (shootingGrid[row, col]) return; // Already shot here

                shootingGrid[row, col] = true; // Mark as shot

                if (placementGrid[row, col])
                {
                    // Hit
                    DrawCross(shootingButtons[row, col], Brushes.Red);
                }
                else
                {
                    // Miss
                    DrawCircle(shootingButtons[row, col], Brushes.Yellow);
                }
            }
        }

        private bool CanPlaceShip(int row, int col, int length, bool horizontal)
        {
            if (horizontal)
            {
                if (col + length > GridSize) return false;
                for (int i = col; i < col + length; i++)
                {
                    if (placementGrid[row, i] || IsAdjacentOccupied(row, i))
                        return false;
                }
            }
            else
            {
                if (row + length > GridSize) return false;
                for (int i = row; i < row + length; i++)
                {
                    if (placementGrid[i, col] || IsAdjacentOccupied(i, col))
                        return false;
                }
            }
            return true;
        }

        private bool IsAdjacentOccupied(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < GridSize && j >= 0 && j < GridSize && placementGrid[i, j])
                        return true;
                }
            }
            return false;
        }

        private void PlaceShip(int row, int col, int length, bool horizontal)
        {
            if (horizontal)
            {
                for (int i = col; i < col + length; i++)
                {
                    placementGrid[row, i] = true;
                    DrawCircle(placementButtons[row, i], Brushes.Gray);
                }
            }
            else
            {
                for (int i = row; i < row + length; i++)
                {
                    placementGrid[i, col] = true;
                    DrawCircle(placementButtons[i, col], Brushes.Gray);
                }
            }
        }

        private void UpdatePreview(int row, int col, int length, bool horizontal, bool isValid)
        {
            ClearPreview();
            if (horizontal)
            {
                for (int i = col; i < col + length; i++)
                {
                    if (i < GridSize)
                    {
                        if (placementGrid[row, i])
                        {
                            // If the cell already contains a ship, skip it
                            continue;
                        }
                        DrawCircle(placementButtons[row, i], isValid ? Brushes.LightGray : Brushes.LightCoral);
                    }
                }
            }
            else
            {
                for (int i = row; i < row + length; i++)
                {
                    if (i < GridSize)
                    {
                        if (placementGrid[i, col])
                        {
                            // If the cell already contains a ship, skip it
                            continue;
                        }
                        DrawCircle(placementButtons[i, col], isValid ? Brushes.LightGray : Brushes.LightCoral);
                    }
                }
            }
        }

        private void ClearPreview()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (!placementGrid[row, col])
                    {
                        // Clear the content of the button if no ship is placed
                        placementButtons[row, col].Content = new Grid();
                    }
                }
            }
        }

        private void DrawCircle(Button button, Brush color)
        {
            Grid grid = new Grid();
            Ellipse circle = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = color,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            grid.Children.Add(circle);
            button.Content = grid;
        }

        private void DrawCross(Button button, Brush color)
        {
            Grid grid = new Grid();
            Line line1 = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 20,
                Y2 = 20,
                Stroke = color,
                StrokeThickness = 2
            };
            Line line2 = new Line
            {
                X1 = 20,
                Y1 = 0,
                X2 = 0,
                Y2 = 20,
                Stroke = color,
                StrokeThickness = 2
            };
            grid.Children.Add(line1);
            grid.Children.Add(line2);
            button.Content = grid;
        }

        private void LockPlacementGrid()
        {
            foreach (Button btn in PlacementGrid.Children)
            {
                btn.IsEnabled = false;
            }
        }

        private void EnableShootingGrid()
        {
            foreach (Button btn in ShootingGrid.Children)
            {
                btn.IsEnabled = true;
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R && !allShipsPlaced)
            {
                // Toggle ship orientation
                isHorizontal = !isHorizontal;
                MessageBox.Show($"Ship orientation changed to {(isHorizontal ? "horizontal" : "vertical")}.");
            }
        }
    }
}