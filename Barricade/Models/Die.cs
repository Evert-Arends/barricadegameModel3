using System;

namespace Barricade.Models
{
    public class Die
    {
        public int MovesRemaining;
        public int ArchivedMovesRemaining;

        public void ThrowDie()
        {
            var r = new Random();
            MovesRemaining = r.Next(1, 7);
            ArchivedMovesRemaining = MovesRemaining;
        }

        public void RemoveOneMove()
        {
            if (MovesRemaining > 0)
            {
                MovesRemaining--;
            }
        }

        public void RevertMoveAmount()
        {
            MovesRemaining = ArchivedMovesRemaining;
        }
    }
}