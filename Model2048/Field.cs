using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model2048
{
#if DEBUG
    public
#endif
    class Field
    {
        private int[,] field;

        public int HSize { get; private set; }
        public int VSize { get; private set; }
        public Field(int size)
        {
            Create(size, size);
        }

        public Field(int hSize, int vSize)
        {
            Create(hSize, vSize);
        }

        private void Create(int hSize, int vSize)
        {
            this.HSize = hSize;
            this.VSize = vSize;
            this.field = new int[hSize, vSize];
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < this.HSize; i++)
                for (int j = 0; j < this.VSize; j++)
                    this.field[i, j] = 0;
        }

        public bool IsOnField(Coordinates coords)
        {
            if ((coords.Horizontal >= 0) && (coords.Vertical >= 0) && (coords.Horizontal < this.HSize) && (coords.Vertical < this.VSize))
                return true;
            else
                return false;
        }

        public bool HaveEmptyCells()
        {
            return HaveCellWithValue(0);
        }

        public bool HaveCellWithValue(int value)
        {
            for (int i = 0; i < this.HSize; i++)
                for (int j = 0; j < this.VSize; j++)
                    if (this.field[i, j] == value)
                        return true;
            return false;
        }

        public List<Coordinates> FindEmptyCells()
        {
            List<Coordinates> result = new List<Coordinates>();
            for (int i = 0; i < this.HSize; i++)
                for (int j = 0; j < this.VSize; j++)
                    if (this.field[i, j] == 0)
                        result.Add(new Coordinates(i, j));
            return result;
        }

        public int Get(Coordinates coords)
        {
            return this.field[coords.Horizontal, coords.Vertical];
        }
        public void Set(Coordinates coords, int value)
        {
            if (IsOnField(coords))
                this.field[coords.Horizontal, coords.Vertical] = value;
        }
    }
}
