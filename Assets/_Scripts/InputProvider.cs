using UnityEngine;
using UnityEngine.InputSystem;

// Simple player input provider using Unity's new™ InputSystem
public class InputProvider : MonoBehaviour
{
    InputSystemActions inputActions;
    InputAction moveAction;
    InputAction lookAction;
    InputAction primaryButtonAction;
    InputAction secondaryButtonAction;
    InputAction crouchAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction escapeAction;


    public float ActionPressedThreshold = 0.9f;
    public PlayerInput GetInput()
    {

        return new PlayerInput()
        {
            move = moveAction.ReadValue<Vector2>(),
            look = lookAction.ReadValue<Vector2>(),
            jumpHeld = jumpAction.ReadValue<float>() > ActionPressedThreshold,
            jumpReleased = jumpAction.WasReleasedThisFrame(),
            jumpPressed = jumpAction.WasPressedThisFrame(),
            crouch = crouchAction.ReadValue<float>() > ActionPressedThreshold,
            sprint = sprintAction.ReadValue<float>() > ActionPressedThreshold,
            escapePressed = escapeAction.WasPressedThisFrame(),
            primaryButtonPressed = primaryButtonAction.WasPressedThisFrame(),
            //primaryButtonHeld = primaryButtonAction.IsInProgress(),
            //primaryButtonReleased = primaryButtonAction.WasReleasedThisFrame(),
            //secondaryButtonPressed = secondaryButtonAction.WasPressedThisFrame(),
            //secondaryButtonHeld = secondaryButtonAction.IsInProgress(),
            //secondaryButtonReleased = secondaryButtonAction.WasReleasedThisFrame(),           
        };


    }
    private void Awake()
    {
        inputActions = new InputSystemActions();
        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
        jumpAction = inputActions.Player.Jump;
        crouchAction = inputActions.Player.Crouch;
        sprintAction = inputActions.Player.Sprint;
        primaryButtonAction = inputActions.Player.Interact;
        //secondaryButtonAction = inputActions.Player.SecondaryButton;
        escapeAction = inputActions.UI.Cancel;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        primaryButtonAction.Enable();
        //secondaryButtonAction.Enable();
        escapeAction.Enable();
        crouchAction.Enable();
        jumpAction.Enable();
        

    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        primaryButtonAction.Disable();
        //secondaryButtonAction.Disable();
        escapeAction.Disable();
        crouchAction.Disable();
        jumpAction.Disable();        
    }

}
