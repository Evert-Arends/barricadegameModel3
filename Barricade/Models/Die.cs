using System;

namespace Barricade.Models
{
    public class Die
    {
        public int ThrowAmount;
        public int ArchivedThrowAmount;

        public void ThrowDie()
        {
            var r = new Random();
            ThrowAmount = r.Next(1, 6);
            ArchivedThrowAmount = ThrowAmount;
        }

        public void RemoveOneEye()
        {
            if (ThrowAmount > 0)
            {
                ThrowAmount--;
            }
        }

        public void RevertThrownAmount()
        {
            ThrowAmount = ArchivedThrowAmount;
        }
    }
}