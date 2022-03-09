using System;
using System.Collections.Generic;

enum CellType
{
    Empty,
    Red,
    Blue
}

class Cell
{
    public CellType Type { get; set; }

    public Cell(CellType type)
    {
        Type = type;
    }
}

class Game
{
    //A multi-dimensional List of cells
    private List<List<Cell>> board = new List<List<Cell>>();
    public string CurrentPlayer { get; set; }

    public Game(int rows, int columns)
    {
        //Create the board with empty cells
        for (int i = 0; i < rows; i++)
        {
            board.Add(new List<Cell>());

            for (int j = 0; j < columns; j++)
            {
                board[i].Add(new Cell(CellType.Empty));
            }
        }

        //Randdomly choose who starts the game
        CurrentPlayer = new Random().Next(0, 2) == 0 ? "Red" : "Blue";
    }

    //Output the board to the console
    private void Print()
    {
        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].Count; j++)
            {
                char boardLetter = ' ';

                //Gets the correct letter andd colour for the board.
                switch (board[i][j].Type)
                {
                    case CellType.Empty:
                        Console.ForegroundColor = ConsoleColor.White;
                        boardLetter = '*';
                        break;
                    case CellType.Red:
                        Console.ForegroundColor = ConsoleColor.Red;
                        boardLetter = 'R';
                        break;
                    case CellType.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        boardLetter = 'B';
                        break;
                }

                Console.Write(boardLetter + " ");
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }

    private bool PlacePiece(int column)
    {
        //Check if the column is full.
        if (board[0][column].Type != CellType.Empty)
        {
            return false;
        }

        //Find the first empty row and place the piece there.
        for (int i = board.Count - 1; i >= 0; i--)
        {
            if (board[i][column].Type == CellType.Empty)
            {
                board[i][column].Type = CurrentPlayer == "Red" ? CellType.Red : CellType.Blue;
                return true;
            }
        }

        return false;
    }

    bool CheckForWinner()
    {
        bool CheckForHorizontalWin()
        {
            for (int i = 0; i < board.Count; i++)
            {
                for (int j = 0; j < board[i].Count - 3; j++)
                {
                    if (board[i][j].Type != CellType.Empty &&
                        board[i][j].Type == board[i][j + 1].Type &&
                        board[i][j].Type == board[i][j + 2].Type &&
                        board[i][j].Type == board[i][j + 3].Type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool CheckForVerticalWin()
        {
            for (int i = 0; i < board.Count - 3; i++)
            {
                for (int j = 0; j < board[i].Count; j++)
                {
                    if (board[i][j].Type != CellType.Empty &&
                        board[i][j].Type == board[i + 1][j].Type &&
                        board[i][j].Type == board[i + 2][j].Type &&
                        board[i][j].Type == board[i + 3][j].Type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool CheckForDiagonalWin()
        {
            //Check for diagonal win from top left to bottom right
            for (int i = 0; i < board.Count - 3; i++)
            {
                for (int j = 0; j < board[i].Count - 3; j++)
                {
                    if (board[i][j].Type != CellType.Empty &&
                        board[i][j].Type == board[i + 1][j + 1].Type &&
                        board[i][j].Type == board[i + 2][j + 2].Type &&
                        board[i][j].Type == board[i + 3][j + 3].Type)
                    {
                        return true;
                    }
                }
            }

            //Check for diagonal win from top right to bottom left
            for (int i = 3; i < board.Count; i++)
            {
                for (int j = 0; j < board[i].Count - 3; j++)
                {
                    if (board[i][j].Type != CellType.Empty &&
                        board[i][j].Type == board[i - 1][j + 1].Type &&
                        board[i][j].Type == board[i - 2][j + 2].Type &&
                        board[i][j].Type == board[i - 3][j + 3].Type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        return CheckForHorizontalWin() || CheckForVerticalWin() || CheckForDiagonalWin();
    }

    public void StartGame()
    {
        while (true)
        {
            Print();

            Console.WriteLine("\nPlayer " + CurrentPlayer + ": Enter a column to place a piece");
            
            while (true)
            {
                int column = -1;

                if (int.TryParse(Console.ReadLine(), out column))
                {
                    column--;

                    if (column >= 0 && column < board[0].Count)
                    {
                        if (PlacePiece(column))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Column is full");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid column");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and " + board[0].Count);
                    column = -1;
                }
            }

            if (CheckForWinner())
            {
                Console.WriteLine("Player " + CurrentPlayer + " wins!");
                break;
            }

            CurrentPlayer = CurrentPlayer == "Red" ? "Blue" : "Red";
            Console.Clear();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game(6, 7);
        game.StartGame();
    }
}