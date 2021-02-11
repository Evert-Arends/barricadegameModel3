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
            // Barricade is not allowed on a FInishField
            if (!base.MoveAllowed(fieldTo, die) || fieldTo is FinishField)
            {
                Console.WriteLine($"Barricade said: false");
                return false;
            }
            return true;
        }
    }
}