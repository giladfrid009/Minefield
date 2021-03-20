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
            field.OnMove += AddMove;
            field.OnEnd += SetResult;
            field.OnReset += Unsubscribe;
        }

        private void AddMove(Field sender, MoveArgs e)
        {
            Moves.Add((new Point(e.Row, e.Col), e.Move));
        }

        private void Unsubscribe(Field sender)
        {
            sender.OnMove -= AddMove;
            sender.OnEnd -= SetResult;
            sender.OnReset -= Unsubscribe;
        }

        private void SetResult(Field sender, ResultArgs e)
        {
            GameResult = e.Result;
            Unsubscribe(sender);
        }
    }
}
