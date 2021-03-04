using System;

namespace Minefield
{
    public class ConsGraphics
    {
        public void Subscribe(Field field)
        {
            field.OnMove += DrawCoord;
            field.OnReset += DrawEmpty;
        }

        public void Unsubscribe(Field field)
        {
            field.OnMove -= DrawCoord;
            field.OnReset -= DrawEmpty;
        }

        private void DrawEmpty(Field sender)
        {
            for (int row = 0; row < sender.Height; row++)
            {
                for (int col = 0; col < sender.Width; col++)
                {
                    DrawCoord(sender, new MoveArgs(row, col, Move.Unflag, Field.Hidden));
                }
            }
        }

        private void DrawCoord(Field sender, MoveArgs e)
        {
            Console.SetCursorPosition(e.Col, e.Row);

            switch (e.Value)
            {
                case Field.Hidden:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write('?'); 
                    break;

                case Field.Mine:
                    Console.ForegroundColor = e.Move == Move.Reveal ? ConsoleColor.Yellow : ConsoleColor.Red;
                    Console.Write('@'); 
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(e.Value); 
                    break;
            };
        }
    }
}
