using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody = null;
    [SerializeField] 
    private PlayerInput _playerInput = null;
    [SerializeField]
    private Camera _camera = null;
    [SerializeField]
    private float _maxVelocity = 1.0f;
    [SerializeField]
    private float _velocityMultiplier = 1.0f;

    private void Start()
    {
        _rigidbody.maxLinearVelocity = _maxVelocity;
    }

    private void FixedUpdate()
    {
        Debug.Log($"{name} rigidbody velocity is {_rigidbody.velocity}");

        if (_playerInput.currentActionMap["Forward"].IsPressed()) MoveRigidbody(transform.forward);
        if (_playerInput.currentActionMap["Backward"].IsPressed()) MoveRigidbody(transform.forward * -1);
        if (_playerInput.currentActionMap["Right"].IsPressed()) MoveRigidbody(transform.right);
        if (_playerInput.currentActionMap["Left"]. IsPressed()) MoveRigidbody(transform.right * -1);
    }

    void MoveRigidbody(Vector3 direction)
    {
        //Debug.Log($"{name} MoveRigidbody invoked: direction is {direction}");
        _rigidbody.AddForce(direction * _velocityMultiplier);
    }

    void OnRotate(InputValue value)
    {
        //Debug.Log($"{name} OnRotate invoked: input value is {value.Get<Vector2>()}");
        Vector2 rotationVec = value.Get<Vector2>();
        Vector3 rotationSum = transform.rotation.eulerAngles;
        rotationSum.y += rotationVec.y;
        Quaternion rotation = Quaternion.LookRotation(rotationSum, Vector3.up);
        _rigidbody.MoveRotation(rotation);
    }
}
