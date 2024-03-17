using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class MoveOption : MonoBehaviour
{
    [HideInInspector] public Button button;
    public bool Selected
    {
        get;
        private set;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Select()
    {
        Selected = true;
        SendMessageUpwards("MoveSelected", this);
    }
}
