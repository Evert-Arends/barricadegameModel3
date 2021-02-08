namespace Barricade.Models
{
    public class PlayerPiece : Piece
    {
        public StartField StartField { get; set; }

        public PlayerPiece(string name)
        {
            Name = name;
        }
    }
}