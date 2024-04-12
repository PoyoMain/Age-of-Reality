using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkimButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [HideInInspector] public bool buttonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}