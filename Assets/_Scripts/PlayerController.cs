using System;
using UnityEngine;


// Simple modular player controller

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController _charController;
    public CharacterController CharController => _charController;
    InputProvider _inputProvider;
    public FPSCameraLook CameraLook;
    public Movement Movement;
    

    private bool _hasControl;
    public void SetHasControl(bool value) => _hasControl = value;

    void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _inputProvider = GetComponent<InputProvider>();
        CameraLook = GetComponentInChildren<FPSCameraLook>();    
        Movement = GetComponentInChildren<Movement>();
    }

    private void Start()
    {       
        _hasControl = true;
    }

    void Update()
    {
        PlayerInput input = _inputProvider.GetInput();

        if (!_hasControl) return;

        CameraLook.CameraLook(input.look);
        Movement.UpdateInput(input);
    }
    public void TeleportTo(Vector3 position)
    {
        _charController.enabled = false;
        this.transform.position = position;
        _charController.enabled = true;
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
