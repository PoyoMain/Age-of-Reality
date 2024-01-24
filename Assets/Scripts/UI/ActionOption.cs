using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionOption : MonoBehaviour
{
    public ActionType actionType; // Action corresponding to this action

    [HideInInspector] public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}

public enum ActionType
{
    Melee,
    Magic
}
