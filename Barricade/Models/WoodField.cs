using System.Collections.Generic;
using System.Linq;

namespace Barricade.Models
{
    public class WoodField : Field
    {
        public WoodField()
        {
            // All fields except this one are only allowed once piece.
            Pieces = new List<Piece>();
        }

        // Woodfield is allowed any amount of pieces.
        public override bool IsNotOccupied()
        {
            return true;
        }
        public override bool Crowded()
        {
            // Woodfield is never crowded.
            return false;
        }

        public override bool IsMoveAllowed()
        {
            return !Pieces.OfType<BarricadePiece>().Any();
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