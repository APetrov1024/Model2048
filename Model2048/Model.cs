using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;

namespace Model2048
{
    public class Model
    {
        public enum Directions { Left, Right, Up, Down };
        private Field field;
        private Random rndNum = new Random();
        private bool isMoved;
        private int targetValue;

        public Coordinates LastGeneratedTileCoords { get; private set; }
        public int Score { get; private set; }
        public int TargetValue
        {
            get
            { 
                return this.targetValue;
            }
            set 
            {
                if (value != this.targetValue)
                    this.targetValue = value;
            }
        }
        public int HSize
        {
            get
            {
                return this.field.HSize;
            }
        }
        public int VSize
        {
            get
            {
                return this.field.VSize;
            }
        }


        public Model(int fieldSize)
        {
            this.field = new Field(fieldSize);
            this.LastGeneratedTileCoords = new Coordinates();
            InitField();
        }

        public Model(int hFieldSize, int vFieldSize)
        {
            this.field = new Field(hFieldSize, vFieldSize);
            this.LastGeneratedTileCoords = new Coordinates();
            InitField();
        }

        private void InitField()
        {
            this.field.Clear();
            GenerateNewTile();
            GenerateNewTile();
            Score = 0;
        }

        public void ClearField()
        {
            InitField();
        }

        public int Get(Coordinates coords)
        {
            return this.field.Get(coords);
        }

        public void Action(Directions direction)
        {
            this.isMoved = false;
            MoveTiles(direction);
            MergeTiles(direction);
            if (this.isMoved && this.field.HaveEmptyCells())
                GenerateNewTile();
        }

        private void AddToScore(int value)
        {
            this.Score += 2 * value;
        }

        public bool IsHaveValue(int value)
        {
            return this.field.HaveCellWithValue(value);
        }

        private bool IsTileCanBeMerged(Coordinates coords)
        {
            List<int> neighbors = new List<int>();
            if (coords.Horizontal - 1 >= 0) 
                neighbors.Add(this.field.Get(new Coordinates(coords.Horizontal - 1, coords.Vertical)));
            if (coords.Horizontal + 1 < this.HSize) 
                neighbors.Add(this.field.Get(new Coordinates(coords.Horizontal + 1, coords.Vertical)));
            if (coords.Vertical - 1 >= 0) 
                neighbors.Add(this.field.Get(new Coordinates(coords.Horizontal, coords.Vertical - 1)));
            if (coords.Vertical + 1 < this.VSize) 
                neighbors.Add(this.field.Get(new Coordinates(coords.Horizontal, coords.Vertical + 1)));
            int curValue = this.field.Get(coords);
            foreach (int value in neighbors)
                if (value == curValue)
                    return true;
            return false;
        }

        public bool IsHasNotMoves()
        {
            if (this.field.HaveEmptyCells())
                return false;
            for (int i = 0; i < this.field.HSize; i++)
                for (int j = 0; j < this.field.VSize; j++)
                {
                    if (IsTileCanBeMerged(new Coordinates(i, j)))
                        return false;
                }
            return true;
        }

        private void MergeTiles(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    MergeTilesUp();
                    break;
                case Directions.Down:
                    MergeTilesDown();
                    break;
                case Directions.Left:
                    MergeTilesLeft();
                    break;
                case Directions.Right:
                    MergeTilesRight();
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void MergeTilesUp()
        {
            for (int i = 0; i < this.field.HSize; i++)
                for (int j = 0; j < this.field.VSize - 1; j++)
                {
                    Coordinates curCoords = new Coordinates(i, j);
                    Coordinates nextCoords = new Coordinates(i, j + 1);
                    int curValue = this.field.Get(curCoords);
                    int nextValue = this.field.Get(nextCoords);
                    if (curValue != 0 && curValue == nextValue)
                    {
                        this.field.Set(curCoords, 2 * curValue);
                        this.field.Set(nextCoords, 0);
                        AddToScore(curValue);
                        this.isMoved = true;
                    }
                }
            // при объединении появились нули между значениями
            MoveTilesUp();
        }

        private void MergeTilesDown()
        {
            for (int i = 0; i < this.field.HSize; i++)
                for (int j = this.field.VSize - 1; j > 0; j--)
                {
                    Coordinates curCoords = new Coordinates(i, j);
                    Coordinates nextCoords = new Coordinates(i, j - 1);
                    int curValue = this.field.Get(curCoords);
                    int nextValue = this.field.Get(nextCoords);
                    if (curValue != 0 && curValue == nextValue)
                    {
                        this.field.Set(curCoords, 2 * curValue);
                        this.field.Set(nextCoords, 0);
                        AddToScore(curValue);
                        this.isMoved = true;
                    }
                }
            // при объединении появились нули между значениями
            MoveTilesDown();
        }

        private void MergeTilesLeft()
        {
            for (int i = 0; i < this.field.HSize - 1; i++)
                for (int j = 0; j < this.field.VSize; j++)
                {
                    Coordinates curCoords = new Coordinates(i, j);
                    Coordinates nextCoords = new Coordinates(i + 1, j);
                    int curValue = this.field.Get(curCoords);
                    int nextValue = this.field.Get(nextCoords);
                    if (curValue != 0 && curValue == nextValue)
                    {
                        this.field.Set(curCoords, 2 * curValue);
                        this.field.Set(nextCoords, 0);
                        AddToScore(curValue);
                        this.isMoved = true;
                    }
                }
            // при объединении появились нули между значениями
            MoveTilesLeft();
        }

        private void MergeTilesRight()
        {
            for (int i = this.field.HSize - 1; i > 0; i--)
                for (int j = 0; j < this.field.VSize; j++)
                {
                    Coordinates curCoords = new Coordinates(i, j);
                    Coordinates nextCoords = new Coordinates(i - 1, j);
                    int curValue = this.field.Get(curCoords);
                    int nextValue = this.field.Get(nextCoords);
                    if (curValue != 0 && curValue == nextValue)
                    {
                        this.field.Set(curCoords, 2 * curValue);
                        this.field.Set(nextCoords, 0);
                        AddToScore(curValue);
                        this.isMoved = true;
                    }
                }
            // при объединении появились нули между значениями
            MoveTilesRight();
        }

        private void MoveTiles(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    MoveTilesUp();
                    break;
                case Directions.Down:
                    MoveTilesDown();
                    break;
                case Directions.Left:
                    MoveTilesLeft();
                    break;
                case Directions.Right:
                    MoveTilesRight();
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void MoveTilesUp()
        {
            for (int i = 0; i < this.field.HSize; i++)
                for (int j = 1; j < this.field.VSize; j++)
                {
                    if (this.field.Get(new Coordinates(i, j)) != 0)
                    {
                        Coordinates newCoords = new Coordinates(i, j - 1);
                        Coordinates curCoords = new Coordinates(i, j);
                        while (this.field.IsOnField(newCoords) && (this.field.Get(newCoords) == 0))
                        {
                            this.field.Set(newCoords, this.field.Get(curCoords));
                            this.field.Set(curCoords, 0);
                            curCoords.Set(newCoords);
                            newCoords.Vertical--;
                            this.isMoved = true;
                        }
                    }
                }
        }

        private void MoveTilesDown()
        {
            for (int i = 0; i < this.field.HSize; i++)
                for (int j = this.field.VSize - 2; j >= 0; j--)
                {
                    if (this.field.Get(new Coordinates(i, j)) != 0)
                    {
                        Coordinates newCoords = new Coordinates(i, j + 1);
                        Coordinates curCoords = new Coordinates(i, j);
                        while (this.field.IsOnField(newCoords) && (this.field.Get(newCoords) == 0))
                        {
                            this.field.Set(newCoords, this.field.Get(curCoords));
                            this.field.Set(curCoords, 0);
                            curCoords.Set(newCoords);
                            newCoords.Vertical++;
                            this.isMoved = true;
                        }
                    }
                }
        }

        private void MoveTilesRight()
        {
            for (int j = 0; j < this.field.VSize; j++)
                for (int i = this.field.HSize - 2; i >= 0; i--)
                {
                    if (this.field.Get(new Coordinates(i, j)) != 0)
                    {
                        Coordinates newCoords = new Coordinates(i + 1, j);
                        Coordinates curCoords = new Coordinates(i, j);
                        while (this.field.IsOnField(newCoords) && (this.field.Get(newCoords) == 0))
                        {
                            this.field.Set(newCoords, this.field.Get(curCoords));
                            this.field.Set(curCoords, 0);
                            curCoords.Set(newCoords);
                            newCoords.Horizontal++;
                            this.isMoved = true;
                        }
                    }
                }
        }

        private void MoveTilesLeft()
        {
            for (int j = 0; j < this.field.VSize; j++)
                for (int i = 1; i < this.field.HSize; i++)
                {
                    if (this.field.Get(new Coordinates(i, j)) != 0)
                    {
                        Coordinates newCoords = new Coordinates(i - 1, j);
                        Coordinates curCoords = new Coordinates(i, j);
                        while (this.field.IsOnField(newCoords) && (this.field.Get(newCoords) == 0))
                        {
                            this.field.Set(newCoords, this.field.Get(curCoords));
                            this.field.Set(curCoords, 0);
                            curCoords.Set(newCoords);
                            newCoords.Horizontal--;
                            this.isMoved = true;
                        }
                    }
                }
        }

        private void GenerateNewTile()
        {
            List<Coordinates> EmptyTilesCoords = this.field.FindEmptyCells();
            if (EmptyTilesCoords.Count() > 0)
            {
                Coordinates newTileCoords = EmptyTilesCoords[rndNum.Next(0, EmptyTilesCoords.Count() - 1)];
                int newValue;
                if (rndNum.Next(0, 100) <= 85)
                    newValue = 2;
                else
                    newValue = 4;
                this.field.Set(newTileCoords, newValue);
                this.LastGeneratedTileCoords.Set(newTileCoords);
            }

        }

        public bool IsWin()
        {
            if (this.IsHaveValue(this.TargetValue))
                return true;
            else
                return false;
        }

        public bool IsFail()
        {
            if (this.IsHasNotMoves() && !this.IsHaveValue(this.targetValue))
                return true;
            else
                return false;
        }

#region AccessMethodsForTests
#if DEBUG
        public Field ForTestsOnly_Field
        {
            get 
            {
                return this.field;
            }
            set
            {
                this.field = value;
            }
        }
#endif
#endregion
    }
}
