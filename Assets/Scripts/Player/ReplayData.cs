using UnityEngine;

public struct ReplayData
{
    public Vector3 position;
    public bool isJumping;
    public bool isFlippedX;
    public bool isDead;

    public ReplayData(Vector3 _position, bool _isJumping, bool _isFlippedX, bool _isDead)
    {
        position = _position;
        isJumping = _isJumping;
        isFlippedX = _isFlippedX;
        isDead = _isDead;

    }
}
