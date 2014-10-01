using System.Collections.Generic;

namespace ImhoNet.Domain.Helper
{
    internal class PotentialUserComparer : IComparer<PotentialUser>
    {
        public int Compare(PotentialUser x, PotentialUser y)
        {
            return x.CorrelationValue.CompareTo(y.CorrelationValue);
        }
    }

    internal class PotentialVideoComparer : IComparer<PotentialVideo>
    {
        public int Compare(PotentialVideo x, PotentialVideo y)
        {
            return y.PotentialRate.CompareTo(x.PotentialRate);
        }
    }
}