using UnityEngine;

public struct ReplayData
{
    public Vector3 position;
    public Vector2 movement;
    public bool isJumping;
    public bool isFlippedX;
    public bool interactThisFrame;
    public bool dieThisFrame;

    public ReplayData(Vector3 _position, Vector2 _movement, bool _isJumping, bool _isFlipX, bool _interactThisFrame, bool _dieThisFrame)
    {
        position = _position;
        movement = _movement;
        isJumping = _isJumping;
        isFlippedX = _isFlipX;
        interactThisFrame = _interactThisFrame;
        dieThisFrame = _dieThisFrame;

    }
}
