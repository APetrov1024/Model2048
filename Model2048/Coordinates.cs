using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model2048
{
    public class Coordinates
    {
        public int Horizontal { get; set; }
        public int Vertical { get; set; }

        public Coordinates() { }

        public Coordinates(int horizontal, int vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        public void Set(Coordinates newCoodinates)
        {
            this.Horizontal = newCoodinates.Horizontal;
            this.Vertical = newCoodinates.Vertical;
        }

        public override string ToString()
        {
            return "H: " + this.Horizontal + "; V: " + this.Vertical;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinates coordinates &&
                   Horizontal == coordinates.Horizontal &&
                   Vertical == coordinates.Vertical;
        }

        public override int GetHashCode()
        {
            var hashCode = 1238135884;
            hashCode = hashCode * -1521134295 + Horizontal.GetHashCode();
            hashCode = hashCode * -1521134295 + Vertical.GetHashCode();
            return hashCode;
        }
    }
}
