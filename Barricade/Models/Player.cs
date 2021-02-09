using System.Collections.Generic;

namespace Barricade.Models
{
    public class Player
    {
        public List<Field> StartFields = new List<Field>(4);
        public List<PlayerPiece> PlayerPieces = new List<PlayerPiece>(4);

        public string GetName()
        {
            return PlayerPieces[0].ToString();
        }
        public PlayerPiece ActivePlayerPiece { get; set; }
    }
}