using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MouseOverFocus : MonoBehaviour, IPointerEnterHandler
{
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        GetComponent<Button>().onClick.AddListener(FindObjectOfType<AudioManager>().PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventSystem.SetSelectedGameObject(gameObject);
    }


}
    