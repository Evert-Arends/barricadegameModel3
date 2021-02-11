using System;

namespace Barricade.Models
{
    public class BarricadePiece : Piece
    {
        public BarricadePiece(string name = "X")
        {
            Name = name;
        }

        public override bool MoveAllowed(Field fieldTo, Die die)
        {
            // Barricade is not allowed on a FinishField
            if (!base.MoveAllowed(fieldTo, die) || fieldTo is FinishField || fieldTo.IsBottomRow)
            {
                return false;
            }
            return true;
        }
    }
}