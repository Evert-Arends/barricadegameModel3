using System.Collections.Generic;
using System.Linq;

namespace Barricade.Models
{
    public class RestField : Field
    {
        public RestField()
        {
            // All fields except woods are only allowed once piece.
            Pieces = new List<Piece>();
        }
        
        
        public override bool IsMoveAllowed()
        {
            if (!IsNotOccupied() && Pieces.FirstOrDefault() is BarricadePiece)
            {
                return false;
            }

            return true;
        }
        
        public override string ToString()
        {
            //Pieces.Count Because there could be a temporary amount in here when some walks over the field
            return Pieces.Count <= 0 ? "R" : Pieces[Pieces.Count - 1].ToString();
        }
    }
}