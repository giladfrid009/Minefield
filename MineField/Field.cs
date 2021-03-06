﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Minefield
{
    public enum GameResult { None, Won, Lost }
    public enum Move { Flag, Unflag, Reveal }

    public class Field
    {
        public const int Mine = -1;
        public const int Hidden = -2;

        public event EventHandler<Field, MoveArgs>? OnMove;
        public event EventHandler<Field, ResultArgs>? OnEnd;
        public event EventHandler<Field>? OnReset;

        public GameData Game { get; }
        public int Height { get; }
        public int Width { get; }

        public int this[int row, int col] => UnsField[row, col];

        private readonly int[,] SolField;
        private readonly int[,] UnsField;
        
        public Field(int height, int width)
        {
            Height = height;
            Width = width;

            SolField = new int[height, width];
            UnsField = new int[height, width];

            Game = new GameData(this);
        }

        public void Reset()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    SolField[row, col] = Hidden;
                    UnsField[row, col] = Hidden;
                }
            }

            OnReset?.Invoke(this);
        }

        private void TestCoord(int row, int col, string method)
        {
            if (row < 0 || row > Height - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} in {method}");
            }

            if (col < 0 || col > Width - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(col), $"{nameof(col)} in {method}");
            }
        }

        public void Flag(int row, int col)
        {
            TestCoord(row, col, nameof(Flag));

            if (UnsField[row, col] != Hidden) return;

            UnsField[row, col] = Mine;

            OnMove?.Invoke(this, new MoveArgs(row, col, Move.Flag, Mine));

            if (Game.HasWon())
            {
                OnEnd?.Invoke(this, new ResultArgs(GameResult.Won));
            }
        }

        public void Unflag(int row, int col)
        {
            TestCoord(row, col, nameof(Unflag));

            if (UnsField[row, col] != Mine) return;

            UnsField[row, col] = Hidden;

            OnMove?.Invoke(this, new MoveArgs(row, col, Move.Unflag, Hidden));

            return;
        }

        public void Reveal(int row, int col)
        {
            TestCoord(row, col, nameof(Reveal));

            if (UnsField[row, col] != Hidden) return;

            UnsField[row, col] = SolField[row, col];

            OnMove?.Invoke(this, new MoveArgs(row, col, Move.Reveal, UnsField[row, col]));

            if (UnsField[row, col] == Mine)
            {
                OnEnd?.Invoke(this, new ResultArgs(GameResult.Lost));          
                return;
            }

            if (UnsField[row, col] == 0)
            {
                foreach ((int nRow, int nCol) in GetAdj(row, col))
                {
                    Reveal(nRow, nCol);
                }
            }

            if (Game.HasWon()) 
            {
                OnEnd?.Invoke(this, new ResultArgs(GameResult.Won));
            }
        }
       
        private IEnumerable<(int Row, int Col)> GetAdj(int row, int col)
        {
            for (int nRow = row - 1; nRow <= row + 1; nRow++)
            {
                if (nRow < 0 || nRow > Height - 1) continue;

                for (int nCol = col - 1; nCol <= col + 1; nCol++)
                {
                    if (nCol < 0 || nCol > Width - 1) continue;

                    if (nRow == row && nCol == col) continue;

                    yield return (nRow, nCol);
                }
            }
        }

        public void Generate(double minePrecent, int originRow, int originCol, int originSize = 1, int? seed = null)
        {
            TestCoord(originRow, originCol, nameof(Generate));

            Random rnd = seed == null ? new Random() : new Random(seed.Value);

            Reset();

            GenOrigin(originRow, originCol, originSize);

            GenMines(minePrecent, rnd);

            GenVals();

            Reveal(originRow, originCol);
        }

        private void GenOrigin(int oRow, int oCol, int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            for (int row = oRow - size; row <= oRow + size; row++)
            {
                if (row < 0 || row > Height - 1) continue;

                for (int col = oCol - size; col <= oCol + size; col++)
                {
                    if (col < 0 || col > Width - 1) continue;

                    SolField[row, col] = 0;
                }
            }
        }

        private void GenMines(double precent, Random rnd)
        {
            if (precent < 0 || precent > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(precent));
            }

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (SolField[row, col] != Hidden) continue;

                    if (rnd.NextDouble() <= precent)
                    {
                        SolField[row, col] = Mine;

                        Game.TotalMines++;
                    }
                }
            }
        }       

        private void GenVals()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (SolField[row, col] == Mine) continue;

                    SolField[row, col] = 0;

                    foreach ((int nRow, int nCol) in GetAdj(row, col))
                    {
                        if (SolField[nRow, nCol] == Mine) SolField[row, col]++;
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder((Width + 1) * Height);

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    int val = UnsField[row, col];

                    switch (val)
                    {
                        case Hidden: builder.Append('?'); break;
                        case Mine:   builder.Append('@'); break;
                        default:     builder.Append(val); break;
                    };
                }

                if (row < Height - 1) builder.Append('\n');
            }

            return builder.ToString();
        }
    }
}
