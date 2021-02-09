using System.Collections.Generic;

namespace Barricade.Models
{
    public class PlayerPiece : Piece
    {
        public StartField StartField { get; set; }
        public Player PieceOwner { get; set; }
        
        public List<Field> TaggedFields { get; set; }

        public PlayerPiece(string name)
        {
            Name = name;
        }
    }
}