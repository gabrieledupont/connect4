using System;

public abstract class Player
{
    public string PlayerName { get; set; }
    public char PlayerID { get; set; }

    public abstract int MakeMove(char[,] board);
}

public class HumanPlayer : Player
{
    public override int MakeMove(char[,] board)
    {
        int dropChoice;

        Console.WriteLine(PlayerName + "'s Turn ");
        do
        {
            Console.WriteLine("Please enter a number between 1 and 7: ");
            dropChoice = Convert.ToInt32(Console.ReadLine());
        } while (dropChoice < 1 || dropChoice > 7);

        while (board[1, dropChoice] == 'X' || board[1, dropChoice] == 'O')
        {
            Console.WriteLine("That column is full, please enter a new column: ");
            dropChoice = Convert.ToInt32(Console.ReadLine());
        }

        return dropChoice;
    }
}

public class Connect4Game
{
    private Player playerOne;
    private Player playerTwo;
    private char[,] board;
    private int full;
    private int win;
    private int again;

    public Connect4Game(Player playerOne, Player playerTwo)
    {
        this.playerOne = playerOne;
        this.playerTwo = playerTwo;
        board = new char[7, 8];
        full = 0;
        win = 0;
        again = 0;
    }

    public void Play()
    {
        Console.WriteLine("Let's Play Connect 4");
        Console.WriteLine("Player One, please enter your name: ");
        playerOne.PlayerName = Console.ReadLine();
        playerOne.PlayerID = 'X';
        Console.WriteLine("Player Two, please enter your name: ");
        playerTwo.PlayerName = Console.ReadLine();
        playerTwo.PlayerID = 'O';

        DisplayBoard();
        do
        {
            int dropChoice = playerOne.MakeMove(board);
            CheckBelow(playerOne, dropChoice);
            DisplayBoard();
            win = CheckFour(playerOne);
            if (win == 1)
            {
                PlayerWin(playerOne);
                again = Restart();
                if (again == 2)
                {
                    break;
                }
            }

            dropChoice = playerTwo.MakeMove(board);
            CheckBelow(playerTwo, dropChoice);
            DisplayBoard();
            win = CheckFour(playerTwo);
            if (win == 1)
            {
                PlayerWin(playerTwo);
                again = Restart();
                if (again == 2)
                {
                    break;
                }
            }
            full = FullBoard();
            if (full == 7)
            {
                Console.WriteLine("The board is full, it is a draw!");
                again = Restart();
            }

        } while (again != 2);
    }

    private void CheckBelow(Player activePlayer, int dropChoice)
    {
        int row = 6;
        while (row > 0 && board[row, dropChoice] != '*')
        {
            row--;
        }
        board[row, dropChoice] = activePlayer.PlayerID;
    }

    private void DisplayBoard()
    {
        int rows = 6;
        int columns = 7;

        for (int row = 1; row <= rows; row++)
        {
            for (int col = 1; col <= columns; col++)
            {
                if (board[row, col] == default(char))
                {
                    board[row, col] = '*';
                }

                Console.Write(board[row, col]);
            }

            Console.WriteLine();
        }
    }

    private int CheckFour(Player activePlayer)
    {
        char XO = activePlayer.PlayerID;
        int win = 0;

        for (int row = 6; row >= 1; row--)
        {
            for (int col = 1; col <= 7; col++)
            {
                if (board[row, col] == XO)
                {
                    if (col <= 4 && board[row, col + 1] == XO && board[row, col + 2] == XO && board[row, col + 3] == XO)
                    {
                        win = 1;
                    }

                    if (row >= 4 && board[row - 1, col] == XO && board[row - 2, col] == XO && board[row - 3, col] == XO)
                    {
                        win = 1;
                    }

                    if (col <= 4 && row >= 4 && board[row - 1, col + 1] == XO && board[row - 2, col + 2] == XO && board[row - 3, col + 3] == XO)
                    {
                        win = 1;
                    }

                    if (col >= 4 && row >= 4 && board[row - 1, col - 1] == XO && board[row - 2, col - 2] == XO && board[row - 3, col - 3] == XO)
                    {
                        win = 1;
                    }
                }
            }
        }

        return win;
    }

    private int FullBoard()
    {
        int full = 0;
        for (int col = 1; col <= 7; col++)
        {
            if (board[1, col] != '*')
                ++full;
        }

        return full;
    }

    private void PlayerWin(Player activePlayer)
    {
        Console.WriteLine(activePlayer.PlayerName + " Connected Four, You Win!");
    }

    private int Restart()
    {
        int restart;

        Console.WriteLine("Would you like to restart? Yes(1) No(2): ");
        restart = Convert.ToInt32(Console.ReadLine());
        if (restart == 1)
        {
            for (int row = 1; row <= 6; row++)
            {
                for (int col = 1; col <= 7; col++)
                {
                    board[row, col] = '*';
                }
            }
        }
        else
        {
            Console.WriteLine("Goodbye!");
        }
        return restart;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Player playerOne = new HumanPlayer();
        Player playerTwo = new HumanPlayer();
        Connect4Game game = new Connect4Game(playerOne, playerTwo);
        game.Play();
    }
}

