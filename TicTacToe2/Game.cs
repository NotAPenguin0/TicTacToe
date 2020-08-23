using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    enum PlayerId
    {
        X,
        O,
        None
    }

    class Game
    {
        private Board board = new Board(3, 3);
        private PlayerId curPlayer = PlayerId.X;

        public Game()
        {
            board.Clear();
        }

        // Plays a turn. If there is a winner, it returns that winner. If the winner is None, that means the game ended in a draw.
        // If the winner is null, the game is still playing.
        public PlayerId? PlayTurn()
        {
            // Print current board
            Display();

            Console.WriteLine($"It's player {curPlayer}'s turn.");

            while (true)
            {
                (uint x, uint y) = GetInputLocation();
                if (board.IsValidLocation(x, y) && board[x, y] == BoardState.Empty)
                {
                    board[x, y] = curPlayer switch
                    {
                        PlayerId.X => BoardState.X,
                        PlayerId.O => BoardState.O,
                        _ => throw new NotImplementedException("Invalid player id")
                    };
                    // Break out of the input loop since we have a valid input
                    break;
                }
                else
                {
                    // Tell the user to input a valid location
                    Console.WriteLine("Please input a valid coordinate");
                }
            }

            curPlayer = NextPlayer();
            PlayerId? winner = CheckWinner();
            if (winner == null && board.IsFull())
            {
                return PlayerId.None;
            }
            return winner;
        }

        // We only need to check the following directions due to the way we iterate over the board
        // - Right
        // - Down
        // - Left-Down diagonal
        // - Right-Down diagonal
        private enum CheckDirection
        {
            Right,
            Down,
            LeftDownDiag,
            RightDownDiag
        }

        private struct DirectionOffset 
        {
            public int X { get; set; }
            public int Y { get; set; }
        };

        private Dictionary<CheckDirection, DirectionOffset> directionMap = new Dictionary<CheckDirection, DirectionOffset>()
        {
            { CheckDirection.Right, new DirectionOffset { X = 1, Y = 0 } },
            { CheckDirection.Down, new DirectionOffset { X = 0, Y = 1 } },
            { CheckDirection.LeftDownDiag, new DirectionOffset { X = -1, Y = 1 } },
            { CheckDirection.RightDownDiag, new DirectionOffset { X = 1, Y = 1} }
        };

        private bool CheckRowAtWithDirection(PlayerId player, uint startX, uint startY, CheckDirection direction)
        {
            DirectionOffset offset = directionMap[direction];
            // You need 3 symbols in a row to win
            const uint RowWinLength = 3;
            for(int i = 0; i < RowWinLength; ++i)
            {
                // Cast to int to avoid signed/unsigned madness
                int xInt = (int)startX + i * offset.X;
                int yInt = (int)startY + i * offset.Y;

                if (xInt < 0 || yInt < 0) { return false; }
                // Cast back to unsigned now that we have verified that the result is positive
                uint x = (uint)xInt;
                uint y = (uint)yInt;

                BoardState playerSymbolState = player switch
                {
                    PlayerId.X => BoardState.X,
                    PlayerId.O => BoardState.O,
                    _ => throw new NotImplementedException("Invalid player to check")
                };

                // Make sure to short-circuit on invalid location.
                // If any of these conditions fail, there is no winning direction here
                if (!board.IsValidLocation(x, y) || board[x, y] != playerSymbolState)
                {
                    return false;
                }
            }
            // If we made it through the loop, there is a winning row here
            return true;
        }

        private bool RowCompletedAt(PlayerId player, uint x, uint y)
        {
            foreach(var direction in Enum.GetValues(typeof(CheckDirection)).Cast<CheckDirection>())
            {
                // Check this direction
                if (CheckRowAtWithDirection(player, x, y, direction)) { return true; }
            }
            return false;
        }

        // Checks if the game has ended. If so, it returns the winner. If not, it returns null
        private PlayerId? CheckWinner()
        {
            // Go over each tile on the board and check if there is a completed row starting  on this tile

            for (uint y = 0; y < board.Height; ++y)
            {
                for (uint x = 0; x < board.Width; ++x)
                {
                    if (RowCompletedAt(PlayerId.O, x, y)) { return PlayerId.O; }
                    if (RowCompletedAt(PlayerId.X, x, y)) { return PlayerId.X; }
                }
            }
            // No winner found, return null
            return null;
        }

        private PlayerId NextPlayer()
        {
            return curPlayer switch { 
                PlayerId.X => PlayerId.O, 
                PlayerId.O => PlayerId.X, 
                _ => throw new NotImplementedException("Invalid player ID")
            };
        }

        private (uint, uint) GetInputLocation()
        {
            try
            {
                Console.WriteLine("Input 2 comma separated coordinates");
                string input = Console.ReadLine();
                // Remove all whitespace
                input = input.Replace(" ", "");
                // Parse into 2 separate strings, each containing a single coordinate
                string[] coords = input.Split(',');
                // Now parse into x and y variables
                uint.TryParse(coords[0], out uint x);
                uint.TryParse(coords[1], out uint y);
                return (x, y);
            }
            catch(Exception)
            {
                return (uint.MaxValue, uint.MaxValue);
            }
        }

        private void Display()
        {
            for (uint y = 0; y < board.Height; ++y)
            {
                for (uint x = 0; x < board.Width; ++x)
                {
                    BoardState value = board[x, y];
                    if (value == BoardState.Empty)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(value.ToString());
                    }
                }
                Console.Write("\n");
            }
        }
    }
}
