using Assets.Scripts;
using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private InputEventHandler<InputEvent> _movementInputHandler = null;
    
    public string MovementActionMapName;

    [SerializeField]
    private Rigidbody _rigidbody = null;
    [SerializeField]
    private Camera _camera = null;

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

    [Header("Control system")]
    [SerializeField]
    private InputActionAsset _inputAsset = null;

    [Header("UIToolkit test")]
    public IReadOnlyList<string> InspectorList = new List<string>()
    {
        "Value1", "Value2", "Value3"
    };
    //public ReadOnlyArray<string> InspectorReadOnlyArray = new ReadOnlyArray<string>(InspectorList.ToArray());
    public int Count = 4;

    private void Start()
    {
        _movementInputHandler = new PlayerInputHandler<InputEvent>(_inputAsset.FindActionMap(MovementActionMapName)); //change
        _movementInputHandler.Initialize();

        InitializeBody();
        InitializeDebugObjects();
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
            return targetRotation.x > _yUpperRotationMax;
        }
        return targetRotation.x < _yLowerRotationMax;
    }

    bool IsWithinUpperRange(Vector3 rotation)
    {
        return rotation.x - 90 > 0;
    }
}
