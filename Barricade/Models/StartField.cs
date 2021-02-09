using System;
using System.Collections.Generic;

namespace Barricade.Models
{
    public class StartField : Field
    {
        public StartField()
        {
            // All fields except woods are only allowed once piece.
            Pieces = new List<Piece>();
        }

        public override string ToString()
        {
            if (Pieces != null && Pieces.Count > 0)
                return Pieces[0].ToString();
            return "S";
        }
    }
}