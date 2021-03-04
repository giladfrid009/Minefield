namespace Minefield
{
    public class GameData
    {
        public int TotalMines { get; internal set; }

        public GameResult Result { get; private set; }
        public int NumHidden { get; private set; }
        public int NumRevealed { get; private set; }
        public int NumFlagged { get; private set; }    

        internal GameData(Field field)
        {
            field.OnMove += AddEntry;
            field.OnEnd += SetResult;
            field.OnReset += ResetData;
        }

        public bool HasWon()
        {
            return Result == GameResult.Won || (Result != GameResult.Lost && NumFlagged == TotalMines && NumHidden == 0);
        }

        private void AddEntry(Field sender, MoveArgs e)
        {
            switch (e.Move)
            {
                case Move.Flag: 
                    NumFlagged++; 
                    NumHidden--; 
                    break;
                
                case Move.Unflag: 
                    NumFlagged--; 
                    NumHidden++; 
                    break;
                
                case Move.Reveal: 
                    NumRevealed++; 
                    NumHidden--; 
                    break;
            }
        }

        private void ResetData(Field sender)
        {
            NumHidden = sender.Width * sender.Height;
            Result = GameResult.None;
            TotalMines = 0;
            NumRevealed = 0;
            NumFlagged = 0;
        }

        private void SetResult(Field sender, ResultArgs e)
        {
            Result = e.Result;
        }
    }
}
