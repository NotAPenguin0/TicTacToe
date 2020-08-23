using TicTacToe.Utils;

namespace TicTacToe
{
    public enum BoardState
    {
        X,
        O,
        Empty
    }

    class Board
    {
        private Array2D<BoardState> board;

        public uint Width { get => board.Width; }
        public uint Height { get => board.Height; }

        public Board(uint width, uint height)
        {
            board = new Array2D<BoardState>(width, height);
        }

        public void Clear()
        {
            for (uint idx = 0; idx < board.Length; ++idx)
            {
                board[idx] = BoardState.Empty;
            }
        }

        public BoardState this[uint x, uint y] {
            get { return board[x, y]; }
            set { board[x, y] = value; }
        }

        public bool IsValidLocation(uint x, uint y)
        {
            return (x < board.Width) && (y < board.Height);
        }

        public bool IsFull()
        {
            for (uint idx = 0; idx < board.Length; ++idx)
            {
                if (board[idx] == BoardState.Empty)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
