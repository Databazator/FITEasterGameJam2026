using System;
using UnityEngine;
using UnityEngine.Windows;
// Simple modular player controller

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    InputProvider _inputProvider;
    public FPSCameraLook CameraLook;
    public Movement Movement;
    public GravityAttractee GravAttractee;
    public GravityAttractionHandler GravityHandler;
    public CapsuleCollider PlayerCollider;    
    public PlayerInteractor PlayerInteractor;

    private bool _hasControl;
    private bool _freezePlayer;
    public void SetHasControl(bool value) => _hasControl = value;

    public void SetFreezePlayer(bool value) => _freezePlayer = value;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputProvider = GetComponent<InputProvider>();
        CameraLook = GetComponentInChildren<FPSCameraLook>();    
        Movement = GetComponentInChildren<Movement>();
        PlayerInteractor = GetComponent<PlayerInteractor>();
        GravAttractee = GetComponent<GravityAttractee>();
        if (!GravityHandler)
        {
            Debug.LogWarning("Player: GravityHandler reference missing!");
            GravityHandler = FindAnyObjectByType<GravityAttractionHandler>();
        }
        _hasControl = true;
    }

    void Update()
    {
        PlayerInput input = _inputProvider.GetInput();

        if (_freezePlayer) return;
        if(!_hasControl)
        {
            input = new PlayerInput { };
        }

        GravityHandler?.UpdateHandler();
        CameraLook.CameraLook(input.look);
        Movement.UpdateMovement(input);
        PlayerInteractor.UpdateInteractor(input.interactButtonPressed);
    }
    
    public void TeleportTo(Vector3 position)
    {
        //_rigidbody.enabled = false;
        this.Rigidbody.position = position;
        //_rigidbody.enabled = true;
    }

    public void SetRotation(Vector3 eulerAngles)
    {
        CameraLook.SetRotation(eulerAngles);
    }

    public Vector3 GetRotation()
    {
        return CameraLook.GetRotation();
    }

}
