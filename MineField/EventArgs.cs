using System;

namespace Minefield
{
    public delegate void EventHandler<TSender, TArgs>(TSender sender, TArgs e) where TArgs : EventArgs;
    public delegate void EventHandler<TSender>(TSender sender);

    public class MoveArgs : EventArgs
    {
        public Move Move { get; }
        public int Row { get; }
        public int Col { get; }
        public int Value { get; }

        public MoveArgs(int row, int col, Move move, int val)
        {
            Row = row;
            Col = col;
            Move = move;
            Value = val;
        }
    }

    public class ResultArgs : EventArgs
    {
        public GameResult Result { get; }

        public ResultArgs(GameResult result)
        {
            Result = result;
        }
    }
}
