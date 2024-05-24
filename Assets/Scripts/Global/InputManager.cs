using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Accessible properties
    private PlayerInput playerInput;

    public float MoveInput;
    public bool JumpInputPressed;
    public bool JumpInputPressing;
    public bool JumpInputReleased;
    public bool PauseInput;
    public bool CancelInput;
    public bool ResetInput;
    public bool AnyInput;

    // Not accessible properties

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction pauseAction;
    private InputAction cancelAction;
    private InputAction resetAction;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        pauseAction = playerInput.actions["Pause"];
        cancelAction = playerInput.actions["Cancel"];
        resetAction = playerInput.actions["Reset"];
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<float>();
        JumpInputPressed = jumpAction.WasPressedThisFrame();
        JumpInputPressing = jumpAction.IsPressed();
        JumpInputReleased = jumpAction.WasReleasedThisFrame();
        PauseInput = pauseAction.WasPressedThisFrame();
        CancelInput = cancelAction.WasPressedThisFrame();
        ResetInput = resetAction.WasPressedThisFrame();
        AnyInput = Input.anyKeyDown;
    }
}