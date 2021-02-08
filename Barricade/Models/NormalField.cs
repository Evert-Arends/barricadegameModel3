using System.Collections.Generic;

namespace Barricade.Models
{
    public class NormalField : Field
    {
        public NormalField()
        {
            // All fields except woods are only allowed once piece.
            Pieces = new List<Piece>(1);
        }

        public override string ToString()
        {
            //Pieces.Count Because there could be a temporary amount in here when some walks over the field
            return TempPiece != null ? TempPiece.ToString() :
                Pieces.Count <= 0 ? "N" : Pieces[Pieces.Count - 1].ToString();
        }
    }
}