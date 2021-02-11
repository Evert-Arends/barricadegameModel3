using System.Collections.Generic;
using System.Dynamic;

namespace Barricade.Models
{
    public abstract class Field
    {
        public List<Piece> Pieces = new List<Piece>();

        public virtual bool IsNotOccupied()
        {
            return Pieces == null || Pieces.Count == 0;
        }

        public bool IsVillage { get; set; }
        public bool IsBottomRow { get; set; }

        public virtual bool IsMoveAllowed()
        {
            return true;
        }

        public virtual bool Crowded()
        {
            return Pieces.Count > 1;
        }

        // The piece that is showed when walking over it, but not staying _permanently_.
        public Field LeftConnectedField { get; set; }
        public Field RightConnectedField { get; set; }
        public Field TopConnectedField { get; set; }
        public Field DownConnectedField { get; set; }

        public abstract override string ToString();
    }
}