using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FocusSwitch : MonoBehaviour
{
    private GameObject focus;
    
    public void LoseFocus()
    {
        focus = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void RestoreFocus()
    {
        EventSystem.current.SetSelectedGameObject(focus);
        focus = null;
    }
}
