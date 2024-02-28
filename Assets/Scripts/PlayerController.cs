using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
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
    [SerializeField, Range(1, 100)]
    private float _xSensitivity = 1.0f;
    [SerializeField, Range(1, 100)]
    private float _ySensitivity = 1.0f;
    [SerializeField, Range(0, 360)]
    private float _yUpperRotationMax = 1.0f;
    [SerializeField, Range(0, 360)]
    private float _yLowerRotationMax = 1.0f;

    [Header("Body parts")]
    private GameObject _head = null;

    [Header("Debug")]
    private GameObject _comRepresentation = null;

    private void Start()
    {
        InitializeRigidbody();
        InitializeBody();
        InitializeDebugObjects();
    }

    void InitializeRigidbody()
    {
        _rigidbody ??= GetComponent<Rigidbody>(); //doesn't work as intended
        _rigidbody.maxLinearVelocity = _maxVelocity;
    }

    private void InitializeBody()
    {
        _head ??= GameObject.Find("HeadGO");
    }

    void InitializeDebugObjects()
    {
        _comRepresentation ??= GameObject.Find("CenterOfMassRespresentation");
        _comRepresentation.transform.position = _rigidbody.centerOfMass;
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
        if (rotationVec == Vector2.zero) return;
        RotateAlongX(rotationVec);
        RotateAlongY(rotationVec);
    }

    void RotateAlongX(Vector2 rotationVec)
    {
        Vector3 xRotation = _rigidbody.rotation.eulerAngles;
        xRotation.y += rotationVec.x;
        Quaternion rotation = Quaternion.Euler(xRotation);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, rotation, Time.deltaTime * _xSensitivity));
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
        if(IsWithinUpperRange(targetRotation))
        {
            Debug.Log($"{name} head rotation is withing upper range boundaries {targetRotation.x > _yUpperRotationMax}");
            return targetRotation.x > _yUpperRotationMax;
        }
        Debug.Log($"{name} head rotation is withing lower range boundaries {targetRotation.x < _yLowerRotationMax}");
        return targetRotation.x < _yLowerRotationMax;
    }

    bool IsWithinUpperRange(Vector3 rotation)
    {
        return rotation.x - 90 > 0;
    }
}
