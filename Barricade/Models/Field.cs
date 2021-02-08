using System.Collections.Generic;
using System.Dynamic;

namespace Barricade.Models
{
    public abstract class Field
    {
        public List<Piece> Pieces = new List<Piece>();
        // The piece that is showed when walking over it, but not staying _permanently_.
        public Piece TempPiece { get; set; }
        public Field LeftConnectedField { get; set; }
        public Field RightConnectedField { get; set; }
        public Field TopConnectedField { get; set; }
        public Field DownConnectedField { get; set; }

        public abstract override string ToString();
    }
}