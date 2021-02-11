using System;
using System.Collections;

namespace Barricade.Models
{
    public abstract class Piece
    {
        protected string Name = "";

        // The field that the piece is on currently
        public Field PieceField { get; set; }

        // The field that the piece starts out on.
        public Field StartOutField { get; set; }

        public virtual bool MoveAllowed(Field fieldTo, Die die)
        {
            return fieldTo != null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}