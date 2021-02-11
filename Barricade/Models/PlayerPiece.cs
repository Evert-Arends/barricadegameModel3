using System.Collections.Generic;

namespace Barricade.Models
{
    public class PlayerPiece : Piece
    {
        public StartField StartField { get; set; }
        public Player PieceOwner { get; set; }

        public List<Field> TaggedFields { get; set; }

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

        public PlayerPiece(string name)
        {
            Name = name;
        }
    }
}