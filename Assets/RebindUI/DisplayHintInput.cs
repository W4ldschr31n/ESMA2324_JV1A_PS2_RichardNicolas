using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DisplayHintInput : MonoBehaviour
{
    public List<GameObject> keyboardHints;
    public List<GameObject> controllerHints;

    private void Update()
    {
        // Unity does not detect D-Pad and Joystick so force their detection
        if (Input.anyKey || SingletonMaster.Instance.InputManager.MoveInput != 0f)
        {
            bool isKeyboard = Keyboard.current.anyKey.isPressed || Keyboard.current.anyKey.wasReleasedThisFrame;
            foreach(GameObject hint in keyboardHints)
            {
                hint.SetActive(isKeyboard);
            }
            foreach (GameObject hint in controllerHints)
            {
                hint.SetActive(!isKeyboard);
            }
        }

    }
}
