using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    internal interface ITracker<T>
    {
        public List<T> Tracked { get; }
    }
}
