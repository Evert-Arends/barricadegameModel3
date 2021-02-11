using System.Collections.Generic;

namespace Barricade.Models
{
    public class PlayerPiece : Piece
    {
        public StartField StartField { get; set; }
        public Player PieceOwner { get; set; }
        
        public override bool MoveAllowed(Field fieldTo, Die die)
        {
            // You're allowed to slay a barricade when it's your last step

            if (!base.MoveAllowed(fieldTo, die) || die.MovesRemaining == 0)
            {
                return false;
            }

            // Check null
            if (fieldTo.IsNotOccupied())
            {
                return true;
            }

            // You're allowed to slay a barricade when it's your last step
            if ((fieldTo.Pieces[0] is BarricadePiece) && die.MovesRemaining > 1)
            {
                return false;
            }

            return true;
        }

        public override Piece NextPiece(Die die)
        {
            // retrieve current position in array and add one to get the next piece.
            var index = PieceOwner.PlayerPieces.FindIndex(a => a == this);
            index++;
            if (index >= PieceOwner.PlayerPieces.Count)
            {
                index = 0;
            }
            // -1 means no piece found
            return index >= 0 ? PieceOwner.PlayerPieces[index] : null;
        }

        public PlayerPiece(string name)
        {
            Name = name;
        }
    }
}