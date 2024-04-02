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
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    internal class FrictionCollidersTracker : MonoBehaviour, IInjectable<PhysicalAgentManager>
    {
        private PhysicalAgentManager _manager = null;
        private Surface _surface = null;
        public Queue<(Collision collision, Surface surface)> CollisionQueue { get; private set; }

        private void Start()
        {
            _surface = GetComponent<Surface>();
            if(_surface == null)
            {
                Debug.LogWarning($"{gameObject.name} friction tracker was unable to find {typeof(Surface)} component");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            TryEnqueueCollision(collision);
        }


        private void OnCollisionStay(Collision collision)
        {
            TryEnqueueCollision(collision);
        }

        private bool TryEnqueueCollision(Collision collision)
        {
            foreach (var item in CollisionQueue)
            {
                if(item.collision.gameObject == collision.gameObject)
                {
                    EnqueueCollision(collision, item.surface);
                    return true;
                }
            }
            Surface surface = null;
            if (HasFrictionPhysics(collision.gameObject, out surface))
            {
                EnqueueCollision(collision, surface);
                return true;
            }
            return false;
        }

        private bool HasFrictionPhysics(GameObject gameObject, out Surface surface)
        {
            surface = gameObject.GetComponent<Surface>();
            return surface != null;
        }

        private void EnqueueCollision(Collision collision, Surface surface)
        {
            Debug.Log($"{gameObject.name} friction tracker added collision with {collision.gameObject.name} to queue");
            CollisionQueue.Enqueue((collision, surface));
        }

        private void FixedUpdate()
        {
            if (CollisionQueue.Count == 0) return;
            if (_manager == null)
            {
                Debug.LogWarning($"{gameObject.name} friction tracker manager is null");
                return;
            }

            (Collision collision, Surface surface) item = default;
            try
            {
                item = CollisionQueue.Dequeue();
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError($"{gameObject.name} friction tracker was unable to dequeue a collision:\n {e.Message}");
                return;
            }

            if (item.collision == null)
            {
                Debug.LogError($"{gameObject.name} friction tracker dequeued collision is null");
                return;
            }

            var slippingForce = FrictionInteractionCalculator.CalculateSlippingForce(item, _manager, _surface);
            if (slippingForce == Vector3.zero) return;
            //slip
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
