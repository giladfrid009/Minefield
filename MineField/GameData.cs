namespace Minefield
{
    public class GameData
    {
        public int TotalMines { get; internal set; }

        public GameResult Result { get; private set; }
        public int NumHidden { get; private set; }
        public int NumOpen { get; private set; }
        public int NumFlags { get; private set; }    

        internal GameData(Field field)
        {
            field.OnMove += Update;
            field.OnEnd += SetResult;
            field.OnReset += ResetData;
        }

        internal bool HasWon()
        {
            return Result == GameResult.Won || (Result != GameResult.Lost && NumFlags == TotalMines && NumHidden == 0);
        }

        private void Update(Field sender, MoveArgs e)
        {
            switch (e.Move)
            {
                case Move.Flag: 
                    NumFlags++;
                    NumHidden--;
                    break;

                case Move.Unflag:
                    NumFlags--; 
                    NumHidden++; 
                    break;
                
                case Move.Reveal: 
                    NumOpen++; 
                    NumHidden--; 
                    break;
            }
        }

        private void ResetData(Field sender)
        {
            NumHidden = sender.Width * sender.Height;
            Result = GameResult.None;
            TotalMines = 0;
            NumOpen = 0;
            NumFlags = 0;
        }

        private void SetResult(Field sender, ResultArgs e)
        {
            Result = e.Result;
        }
    }
}
