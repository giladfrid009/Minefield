using System;

namespace Minefield
{
    public class ConsGraphics
    {
        public void Subscribe(Field field)
        {
            field.OnMove += OnMove;
        }

        public void Unsubscribe(Field field)
        {
            field.OnMove -= OnMove;
        }

        private void OnMove(Field sender, MoveArgs e)
        {
            Console.SetCursorPosition(e.Row, e.Col);

            switch (e.CoordVal)
            {
                case Field.Hidden:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(' '); 
                    break;

                case Field.Mine:
                    Console.ForegroundColor = e.Move == Move.Reveal ? ConsoleColor.Yellow : ConsoleColor.Red;
                    Console.Write('@'); 
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(e.CoordVal); 
                    break;
            };  
        }
    }
}
