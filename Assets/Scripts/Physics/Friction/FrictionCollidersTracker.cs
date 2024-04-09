using Assets.Scripts.Dependency_Injection;
using Assets.Scripts.Physics.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Friction
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Surface))]
    internal class FrictionCollidersTracker : MonoBehaviour, IInjectable<PhysicalAgentManager>
    {
        private PhysicalAgentManager _manager = null;
        private Surface _surface = null;

        public Queue<(IEnumerable<ContactPoint> contacts, Surface surface)> CollisionQueue { get; private set; }

        private void Start()
        {
            _surface = GetComponent<Surface>();
            if(_surface == null)
            {
                Debug.LogWarning($"{gameObject.name} friction tracker was unable to find {typeof(Surface)} component");
            }

            CollisionQueue = new();
        }

        public bool TryEnqueueCollision(Collider collider, IEnumerable<ContactPoint> contacts)
        {
            foreach (var item in CollisionQueue)
            {
                if (item.surface.gameObject == collider.gameObject)
                {
                    EnqueueCollision(contacts, item.surface);
                    return true;
                }
            }
            Surface surface = null;
            if (HasFrictionPhysics(collider.gameObject, out surface))
            {
                EnqueueCollision(contacts, surface);
                return true;
            }
            return false;
        }

        private bool HasFrictionPhysics(GameObject gameObject, out Surface surface)
        {
            surface = gameObject.GetComponent<Surface>();
            return surface != null;
        }

        private void EnqueueCollision(IEnumerable<ContactPoint> contacts, Surface surface)
        {
            Debug.Log($"{gameObject.name} friction tracker added collision with {surface.gameObject.name} to queue");
            CollisionQueue.Enqueue((contacts, surface));
        }

        private void FixedUpdate()
        {
            if (CollisionQueue.Count == 0) return;
            if (_manager == null)
            {
                Debug.LogWarning($"{gameObject.name} friction tracker manager is null");
                return;
            }

            (IEnumerable<ContactPoint> contacts, Surface surface) item = default;
            try
            {
                item = CollisionQueue.Dequeue();
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError($"{gameObject.name} friction tracker was unable to dequeue a collision:\n {e.Message}");
                return;
            }

            if (item.contacts == null)
            {
                Debug.LogError($"{gameObject.name} friction tracker dequeued collision is null");
                return;
            }

            var slippingForce = FrictionInteractionCalculator.CalculateSlippingForce(item.contacts, item.surface, _manager, _surface);
            if (slippingForce == Vector3.zero) return;
            Debug.Log($"{gameObject.name} is slipping!");
            throw new NotImplementedException("Slipping logic not implemented completely");
        }

        public void Inject(PhysicalAgentManager instance)
        {
            if(_manager != null)
            {
                Debug.LogWarning($"{gameObject.name} friction tracker is being reinjected with manager from {instance.gameObject.name}");
                return;
            }
            _manager = instance;
        }
    }
}
