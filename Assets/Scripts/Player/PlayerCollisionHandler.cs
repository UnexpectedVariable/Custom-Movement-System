using Assets.Scripts.Physics.Friction;
using Assets.Scripts.Util.Strategy.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerCollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private Collider[] _footColliders = null;

        private Dictionary<Collider, IEnumerable<ICollisionStrategy>> _strategyMap = null;

        private void Start()
        {
            _strategyMap = new();

            List<ICollisionStrategy> footStrategies = new()
            {
                new SupportCollisionStrategy(),
                new FrictionCollisionStrategy()
            };
            _strategyMap.Add(_footColliders[0], footStrategies);
            _strategyMap.Add(_footColliders[1], footStrategies);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"{gameObject.name} collision handler detected collision enter");
            IEnumerable<(Collider collider, IEnumerable<ContactPoint> contacts)> perColliderContacts = EnumeratePerColliderContacts(collision);

            foreach (var pair in perColliderContacts)
            {
                if(pair.contacts.Count() == 0) continue;
                foreach(var strategy in _strategyMap[pair.collider])
                {
                    strategy.HandleCollisionEnter(pair.contacts);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            IEnumerable<(Collider collider, IEnumerable<ContactPoint> contacts)> perColliderContacts = EnumeratePerColliderContacts(collision);

            foreach (var pair in perColliderContacts)
            {
                if (pair.contacts.Count() == 0) continue;
                foreach (var strategy in _strategyMap[pair.collider])
                {
                    strategy.HandleCollisionStay(pair.contacts);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            IEnumerable<(Collider collider, IEnumerable<ContactPoint> contacts)> perColliderContacts = EnumeratePerColliderContacts(collision);

            foreach (var pair in perColliderContacts)
            {
                if (pair.contacts.Count() == 0) continue;
                foreach (var strategy in _strategyMap[pair.collider])
                {
                    strategy.HandleCollisionExit(pair.contacts);
                }
            }
        }

        private IEnumerable<(Collider collider, IEnumerable<ContactPoint> contacts)> EnumeratePerColliderContacts(Collision collision)
        {
            List<ContactPoint> contacts = new List<ContactPoint>(collision.contactCount);
            collision.GetContacts(contacts);

            List<Collider> handledColliders = _strategyMap.Keys.ToList();
            var perColliderContacts = new List<(Collider collider, IEnumerable<ContactPoint> contacts)>(handledColliders.Count);
            foreach (var collider in handledColliders)
            {
                perColliderContacts.Add((collider, contacts.Where((contact) => contact.thisCollider == collider)));
            }

            return perColliderContacts;
        }
    }
}
