using System.Collections;

namespace Barricade.Models
{
    public abstract class Piece
    {
        protected string Name = "";
        public Field PieceField { get; set; }

        public Field StartOutField { get; set; }

        public Stack FieldsWalked = new Stack();

        public override string ToString()
        {
            return Name;
        }
    }
}