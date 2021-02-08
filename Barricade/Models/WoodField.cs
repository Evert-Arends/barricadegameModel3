using System.Collections.Generic;

namespace Barricade.Models
{
    public class WoodField: Field
    {
        public WoodField()
        {
            // All fields except this one are only allowed once piece.
            Pieces = new List<Piece>();
        }
        public override string ToString()
        {
            if (Pieces.Count <= 0) return "W";
            
            var pieces = "";
            foreach (var piece in Pieces)
            {
                pieces += piece.ToString();
            }

            return pieces;
        }
    }
}