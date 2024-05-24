using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Accessible properties
    private PlayerInput playerInput;

    public float MoveInput;
    public bool JumpInputPressed;
    public bool JumpInputReleased;
    public bool PauseInput;

    // Not accessible properties

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction pauseAction;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        pauseAction = playerInput.actions["Pause"];
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<float>();
        JumpInputPressed = jumpAction.WasPressedThisFrame();
        JumpInputReleased = jumpAction.WasReleasedThisFrame();
        PauseInput = pauseAction.WasPressedThisFrame();
    }
}