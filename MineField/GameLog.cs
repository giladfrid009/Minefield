using System.Collections.Generic;
using System.Drawing;

namespace Minefield
{
    public class GameLog
    {
        public GameResult GameResult { get; private set; } = GameResult.None;

        public List<(Point Coord, Move Move)> Moves = new();

        public GameLog(Field field)
        {
            field.OnMove += OnMove;
            field.OnEnd += OnEnd;
            field.OnReset += OnReset;
        }

        private void OnMove(Field sender, MoveArgs e)
        {
            Moves.Add((new Point(e.Row, e.Col), e.Move));
        }     

        private void OnReset(Field sender)
        {
            sender.OnMove -= OnMove;
            sender.OnEnd -= OnEnd;
            sender.OnReset -= OnReset;
        }

        private void OnEnd(Field sender, ResultArgs e)
        {
            GameResult = e.Result;
            OnReset(sender);
        }
    }
}
