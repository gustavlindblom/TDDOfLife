﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GRID_SIZE = 40;
        private GameBoard _board;
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();

            SetUp();
        }

        private void SetUp()
        {
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            _board = new GameBoard(GRID_SIZE, GRID_SIZE);

            for (int i = 0; i < GRID_SIZE; i++)
            {
                Board.ColumnDefinitions.Add(new ColumnDefinition());
                Board.RowDefinitions.Add(new RowDefinition());
            }

            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 20;
                    ellipse.Height = 20;
                    ellipse.Fill = Brushes.Blue;
                    ellipse.DataContext = _board.GetCell(x, y);
                    ellipse.SetBinding(OpacityProperty, "Alive");
                    Grid.SetRow(ellipse, y);
                    Grid.SetColumn(ellipse, x);

                    Board.Children.Add(ellipse);
                }
            }

            _timer = new Timer(200);
            _timer.Elapsed += (s, e) =>
            {
                _board.StepSimulation();
            };
        }

        private void Board_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_timer.Enabled) return;
            if (e.OriginalSource is Ellipse)
            {
                Ellipse ellipse = (Ellipse)e.OriginalSource;
                Cell c = (Cell)ellipse.DataContext;
                c.Alive = !c.Alive;
            }
        }

        private void ToggleSimulationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.Enabled)
                _timer.Stop();
            else
                _timer.Start();

            ToggleSimulationBtn.Content = _timer.Enabled ? "Stop" : "Start";
        }


    }
}
