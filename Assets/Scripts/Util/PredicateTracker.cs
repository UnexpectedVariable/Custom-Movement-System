using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    internal class PredicateTracker<T> : ITracker<T>
    {
        public List<T> Tracked
        {
            get;
            private set;
        }

        protected Predicate<T> trackPredicate = T => { return T != null; };

        public PredicateTracker()
        {
            Tracked = new List<T>();
        }

        public PredicateTracker(Predicate<T> predicate) : this()
        {
            trackPredicate = predicate;
        }

        public bool Add(T trackable)
        {
            if(!trackPredicate.Invoke(trackable)) return false;
            Debug.Log($"Object of type {typeof(T)} added to tracker");
            Tracked.Add(trackable);
            return true;
        }
        public bool Remove(T trackable)
        {
            Debug.Log($"Object of type {typeof(T)} removed from tracker");
            return Tracked.Remove(trackable);
        }
        public bool Validate(T trackable)
        {
            Debug.Log($"Validating object of type {typeof(T)} in tracker");
            if(!Tracked.Contains(trackable)) throw new ArgumentException("Argument to validate not found among trackable objects");
            if(trackPredicate.Invoke(trackable)) return true;
            Remove(trackable);
            return false;
        }
    }
}
