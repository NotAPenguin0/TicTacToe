namespace TicTacToe
{
    public class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var winner = game.PlayTurn();
            // While there is no winner, keep playing
            while(winner == null)
            {
                winner = game.PlayTurn();
            }

            if (winner == PlayerId.None)
            {
                System.Console.WriteLine("Game Over! The game ended in a draw.");
            }
            else
            {
                System.Console.WriteLine($"Game Over! Player: {winner} won!");
            }
        }
    }
}
