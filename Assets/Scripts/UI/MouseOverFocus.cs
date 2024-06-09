using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseOverFocus : MonoBehaviour, IPointerEnterHandler
{
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventSystem.SetSelectedGameObject(gameObject);
    }

}
    