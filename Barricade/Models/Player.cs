using System.Collections.Generic;

namespace Barricade.Models
{
    public class Player
    {
        public readonly List<Field> StartFields = new List<Field>(4);
        public readonly List<PlayerPiece> PlayerPieces = new List<PlayerPiece>(4);

        public string GetName()
        {
            return PlayerPieces[0]?.ToString();
        }

        public bool OwnsPiece(PlayerPiece piece)
        {
            return PlayerPieces.Contains(piece);
        }
        public PlayerPiece ActivePlayerPiece { get; set; }
    }
}