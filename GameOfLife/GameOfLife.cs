using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    public class GameOfLife
    {
        public GameOfLife(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Grid = new Status[Rows, Columns];
        }

        public bool Stop = false;

        public int Rows { get; }
        public int Columns { get; }
        
        private Status[,] Grid { get; set; }

        private void Init()
        {
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    Grid[row, column] = (Status)RandomNumberGenerator.GetInt32(0, 2);
                }
            }
        }

        private void Print(int timeout = 500)
        { 
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    var cell = Grid[row, column];
                    if (cell == Status.Alive)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                   // Console.Write(" ");
                    Console.Write(cell == Status.Alive ? "A" : "D"); 
                }
                Console.Write("\n"); 
            }
            
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0); 
            Thread.Sleep(timeout);
        }

        public void Start()
        {
            Init();
            while (!Stop)
            {
                Print();
                Generate();
            }
        }

        private void Generate()
        {
            var nextGrid = new Status[Rows, Columns];
             
            for (var row = 1; row < Rows - 1; row++)
            for (var column = 1; column < Columns - 1; column++)
            {
                var aliveNeighborsCount = 0; 
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        if (Grid[row + i, column + j] == Status.Alive)
                            aliveNeighborsCount++; 
                    }
                }
                
                var currentCell = Grid[row, column];
                if (currentCell == Status.Alive)
                {
                    aliveNeighborsCount--;
                } 
                
                // 1. Any live cell with fewer than two live neighbours dies, as if caused by under­population.
                if (currentCell.HasFlag(Status.Alive) && aliveNeighborsCount < 2)
                {
                    nextGrid[row, column] = Status.Dead;
                }

                // 3. Any live cell with more than three live neighbours dies, as if by overcrowding
                else if (currentCell.HasFlag(Status.Alive) && aliveNeighborsCount > 3)
                {
                    nextGrid[row, column] = Status.Dead;
                }

                // 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                else if (currentCell.HasFlag(Status.Dead) && aliveNeighborsCount == 3)
                {
                    nextGrid[row, column] = Status.Alive;
                }
                // 2. Any live cell with two or three live neighbours lives on to the next generation.
                else
                {
                    nextGrid[row, column] = currentCell;
                }
            }

            Grid = nextGrid; 
        }
    }
}
