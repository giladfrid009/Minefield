using System;
using System.Text;

namespace Minefield
{
    public class ConsGraphics
    {
        private readonly string EmptyField;

        public ConsGraphics(Field field)
        {
            Console.CursorVisible = false;

            field.OnMove += DrawCoord;
            field.OnReset += DrawEmpty;

            EmptyField = GenEmpty(field);
        }

        public void Unsubscribe(Field field)
        {
            field.OnMove -= DrawCoord;
            field.OnReset -= DrawEmpty;
        }

        private static string GenEmpty(Field field)
        {
            StringBuilder builder = new();

            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    builder.Append(GetChar(Field.Hidden));
                }

                if(row < field.Height - 1) builder.Append('\n');
            }

            return builder.ToString();
        }

        private void DrawEmpty(Field sender)
        {
            Console.SetCursorPosition(0, 0);

            MoveArgs e = new(0, 0, Move.Unflag, Field.Hidden);

            Console.ForegroundColor = GetColor(e);

            Console.Write(EmptyField);
        }

        private void DrawCoord(Field sender, MoveArgs e)
        {
            Console.SetCursorPosition(e.Col, e.Row);

            Console.ForegroundColor = GetColor(e);

            Console.Write(GetChar(e.Value));
        }

        private static ConsoleColor GetColor(MoveArgs e)
        {
            return e.Value switch
            {
                Field.Mine when e.Move == Move.Reveal => ConsoleColor.Yellow,

                Field.Mine when e.Move != Move.Reveal => ConsoleColor.Red,

                _ => ConsoleColor.Gray
            };
        }

        private static char GetChar(int value)
        {
            return value switch
            {
                Field.Hidden => '?',

                Field.Mine => '@',

                _ => (char)(value + '0'),
            };
        }
    }
}
