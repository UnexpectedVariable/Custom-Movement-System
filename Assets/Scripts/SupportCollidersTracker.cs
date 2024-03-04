using Assets.Scripts;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using IObserver = Assets.Scripts.IObserver<Assets.Scripts.SupportCollidersTracker>;
using IObserved = Assets.Scripts.IObserved<Assets.Scripts.SupportCollidersTracker>;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    internal class SupportCollidersTracker : MonoBehaviour, IObserved
    {
        public List<Collider> SupportColliders {get; private set;}
        private List<IObserver> _observers = null;
        //private EventBinding<>
        [SerializeField]
        [Range(0f, 180f)]
        private float _maxSupportAngle = 0f;

        private void Awake()
        {
            SupportColliders = new List<Collider>();
            _observers = new List<IObserver>();
        }

        private void Start()
        {
            Attach(GetComponents<IObserver>().ToList());
        }

        private bool ProvidesSupport(Collision collision)
        {
            List<ContactPoint> contacts = new List<ContactPoint>(collision.contactCount);
            collision.GetContacts(contacts);
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 innerNormal = contact.normal * -1;
                if (Vector3.Angle(Physics.gravity, innerNormal) < _maxSupportAngle) return true;
            }
            return false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (ProvidesSupport(collision))
            {
                AddSupport(collision.collider);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if(SupportColliders.Contains(collision.collider))
            {
                if (!ProvidesSupport(collision))
                {
                    RemoveSupport(collision.collider);
                }
            }
            else
            {
                if (ProvidesSupport(collision))
                {
                    AddSupport(collision.collider);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (SupportColliders.Contains(collision.collider))
            {
                RemoveSupport(collision.collider);
            }
        }

        private void AddSupport(Collider collider)
        {
            Debug.Log($"Collider {collider.gameObject.name} added as support for {gameObject.name}");
            SupportColliders.Add(collider);
            Notify();
        }

        private void RemoveSupport(Collider collider)
        {
            Debug.Log($"Collider {collider.gameObject.name} removed from supports of {gameObject.name}");
            SupportColliders.Remove(collider);
            Notify();
        }

        public void Attach(IObserver observer)
        {
            Debug.Log($"{gameObject.name} support tracker attached new observer");
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            Debug.Log($"{gameObject.name} support tracker detached observer");
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Handle(this);
            }
        }

        public void Attach(ICollection<IObserver<SupportCollidersTracker>> observers)
        {
            Debug.Log($"{gameObject.name} support tracker attached {observers.Count} new observers");
            _observers.AddRange(observers);
        }

        public void Detach(ICollection<IObserver<SupportCollidersTracker>> observers)
        {
            Debug.Log($"{gameObject.name} support tracker detached {observers.Count} observers");
            throw new NotImplementedException();
        }
    }
}
