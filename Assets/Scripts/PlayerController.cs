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

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody = null;

    [Header("Debug")]
    private GameObject _comRepresentation = null;

    [Header("Control system")]
    [SerializeField]
    private InputActionAsset _inputAsset = null;
    [SerializeField]
    private List<PlayerInputBinder> _playerInputBinders = null;
    [SerializeField]
    private PlayerMovementController _playerMovementController = null;
    public List<PlayerInputBinder> PlayerInputBinders { get => _playerInputBinders; }
    public PlayerMovementController PlayerMovementController { get => _playerMovementController; }

    private void Start()
    {
        InitializeDebugObjects();
    }

    void InitializeDebugObjects()
    {
        _comRepresentation ??= GameObject.Find("CenterOfMassRespresentation");
        _comRepresentation.transform.position = _rigidbody.centerOfMass;
    }
}
