using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util.Comparers
{
    public class UnorderedPairEqualityComparer<T> : IEqualityComparer<(T, T)>
    {
        public bool Equals((T, T) x, (T, T) y)
        {
            return (x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2)) ||
                   (x.Item1.Equals(y.Item2) && x.Item2.Equals(y.Item1));
        }

        public int GetHashCode((T, T) pair)
        {
            unchecked
            {
                return pair.Item1.GetHashCode() ^ pair.Item2.GetHashCode();
            }
        }
    }
}
