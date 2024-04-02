using Assets.Scripts.Events;
using System;
using UnityEngine;

namespace Assets.Scripts.MovementSystem.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class PlayerRotationController : MonoBehaviour, IRotationController
    {
        private EventBinding<InputEvent> _rotationEventBinding;
        private Rigidbody _rb;

        [Header("Rotation parameters")]
        [SerializeField, Range(1, 100)]
        private float _xSensitivity = 1.0f;
        [SerializeField, Range(1, 100)]
        private float _ySensitivity = 1.0f;
        [SerializeField, Range(0, 360)]
        private float _yUpperRotationMax = 1.0f;
        [SerializeField, Range(0, 360)]
        private float _yLowerRotationMax = 1.0f;

        [SerializeField]
        private GameObject _head = null;
        [SerializeField]
        private GameObject _camera = null;


        private void Awake()
        {
            _rotationEventBinding = new EventBinding<InputEvent>(HandleRotation);
            _rb ??= GetComponent<Rigidbody>();
        }

        private void HandleRotation(object sender, InputEvent e)
        {
            var context = e.InputContext;
            if (context.valueType != typeof(Vector2)) return;


            Debug.Log("Rotation input detected!");
            Vector2 rotationVec = context.ReadValue<Vector2>();
            if (rotationVec == Vector2.zero) return;

            RotateAlongX(rotationVec);
            RotateAlongY(rotationVec);
        }

        private void OnEnable()
        {
            EventBus<InputEvent>.Register(_rotationEventBinding);
        }

        private void OnDisable()
        {
            EventBus<InputEvent>.Deregister(_rotationEventBinding);
        }

        void RotateAlongX(Vector2 rotationVec)
        {
            Vector3 xRotation = _rb.rotation.eulerAngles;
            xRotation.y += rotationVec.x;
            Quaternion rotation = Quaternion.Euler(xRotation);
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, rotation, Time.deltaTime * _xSensitivity));
        }

        void RotateAlongY(Vector2 rotationVec)
        {
            Vector3 yRotation = _head.transform.rotation.eulerAngles;
            yRotation.x -= rotationVec.y; //minus because otherwise inverted
            Quaternion rotation = Quaternion.Euler(yRotation);
            if (!IsWithinRotationBoundaries(yRotation)) return;
            _head.transform.rotation = Quaternion.RotateTowards(_head.transform.rotation, rotation, Time.deltaTime * _ySensitivity);
            _camera.transform.rotation = _head.transform.rotation;
        }

        bool IsWithinRotationBoundaries(Vector3 targetRotation)
        {
            if (IsWithinUpperRange(targetRotation))
            {
                Debug.Log($"Target rotation is within upper range noundaries: {targetRotation.x > _yUpperRotationMax}");
                return targetRotation.x > _yUpperRotationMax;
            }
            Debug.Log($"Target rotation is within lower range");
            return targetRotation.x < _yLowerRotationMax;
        }

        bool IsWithinUpperRange(Vector3 rotation)
        {
            return rotation.x - 90 > 0;
        }
    }
}
