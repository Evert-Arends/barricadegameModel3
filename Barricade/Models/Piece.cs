using System;
using System.Collections;
using System.Collections.Generic;

namespace Barricade.Models
{
    public abstract class Piece
    {
        protected string Name = "";
        public List<Field> VisitedFields { get; set; }

        // The field that the piece is on currently
        public Field PieceField { get; set; }

        // The field that the piece starts out on.
        public Field StartOutField { get; set; }

        public virtual bool MoveAllowed(Field fieldTo, Die die)
        {
            if (fieldTo != null)
            {
                if (VisitedFields != null)
                {
                    return !VisitedFields.Contains(fieldTo);
                }

                return true;
            }

            return false;
        }


        public virtual Piece NextPiece(Die die)
        {
            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}