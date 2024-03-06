using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSelectMenu : MonoBehaviour
{
    private List<ActionOption> actionButtons;
    private ActionOption selectedAction;
    private int actionIndex;

    [SerializeField] private MoveSelectMenu moveSelectMenu; // The sub menu to select moves
    [HideInInspector] public ScriptableAttack chosenAttack = null; // The attack chosen from the move select menu
    [HideInInspector] public ScriptableItem chosenItem = null;
    private MoveOption selectedMove;
    private List<MoveOption> moveButtons;
    private int moveIndex;

    private PlayerControls playerControls;
    private PlayerControls.BattleControlsActions battleControls;
    
    private MenuState menuState;

    private void Awake()
    {
        playerControls = new PlayerControls();
        battleControls = playerControls.BattleControls;

        actionButtons = GetComponentsInChildren<ActionOption>().ToList();
        moveButtons = moveSelectMenu.moveButtons;
        moveIndex = moveSelectMenu.moveIndex;

        if (actionButtons.Count == 0) Debug.LogError($"No children with MoveOption script on {gameObject.name} gameobject");

        selectedAction = actionButtons[0];
        EventSystem.current.SetSelectedGameObject(selectedAction.gameObject);
    }

    

    private void Update()
    {
        switch (menuState)
        {
            case MenuState.ActionMenu:
                break;
            case MenuState.SecondaryMenu:
                moveSelectMenu.moveIndex = moveIndex;
                break;
        }
    }

    /// <summary>
    /// Change the state of the menu
    /// </summary>
    /// <param name="newState">The new state to change too</param>
    void ChangeState(MenuState newState)
    {
        menuState = newState;

        switch (newState)
        {
            case MenuState.ActionMenu:
                moveSelectMenu.gameObject.SetActive(false);

                foreach (ActionOption option in actionButtons)
                {
                    option.button.interactable = true;
                }
                break;
            case MenuState.SecondaryMenu:
                moveSelectMenu.menuType = moveSelectMenu.currentUnit.data.Ability;
                moveSelectMenu.actionChosen = selectedAction.actionType;
                moveSelectMenu.gameObject.SetActive(true);
                
                foreach (ActionOption option in actionButtons)
                {
                    option.button.interactable = false;
                }

                moveIndex = 0;
                break;
        }
    }

    /// <summary>
    /// Navigate up the menu
    /// </summary>
    public void SelectUp()
    {
        switch (menuState)
        {
            case MenuState.ActionMenu:
                actionIndex = (actionIndex - 1) < 0 ? actionButtons.Count - 1 : actionIndex - 1;
                selectedAction = actionButtons[actionIndex];
                EventSystem.current.SetSelectedGameObject(selectedAction.gameObject);
                break;
            case MenuState.SecondaryMenu:
                moveIndex = (moveIndex - 1) < 0 ? moveButtons.Count - 1 : moveIndex - 1;
                selectedMove = moveButtons[moveIndex];
                EventSystem.current.SetSelectedGameObject(selectedMove.gameObject);
                break;
        }
        
    }

    /// <summary>
    /// Navigate down the menu
    /// </summary>
    public void SelectDown()
    { 
        switch (menuState)
        {
            case MenuState.ActionMenu:
                actionIndex = (actionIndex + 1) % actionButtons.Count;
                selectedAction = actionButtons[actionIndex];
                EventSystem.current.SetSelectedGameObject(selectedAction.gameObject);
                break;
            case MenuState.SecondaryMenu:
                moveIndex = (moveIndex + 1) % moveButtons.Count;
                selectedMove = moveButtons[moveIndex];
                EventSystem.current.SetSelectedGameObject(selectedMove.gameObject);
                break;
        }
    }

    public void Select()
    {
        switch (menuState)
        {
            case MenuState.ActionMenu:
                if (actionButtons[actionIndex].button.interactable) ChangeState(MenuState.SecondaryMenu);
                break;
            case MenuState.SecondaryMenu:
                switch (selectedAction.actionType)
                {
                    case ActionType.Attack:
                        chosenAttack = (moveSelectMenu.moveButtons[moveIndex] as AttackOption).Attack;
                        ChangeState(MenuState.ActionMenu);
                        break;

                    case ActionType.Item:
                        chosenItem = (moveSelectMenu.moveButtons[moveIndex] as ItemOption).item;
                        ChangeState(MenuState.ActionMenu);
                        break;
                }
                break;
        }
    }

    public void SetCurrentUnit(HeroUnitBase hero)
    {
        if (GameManager.Instance.ItemInventory.IsEmpty) actionButtons[1].button.interactable = false;
        else actionButtons[1].button.interactable = true;

        moveSelectMenu.currentUnit = hero;
    }

    private void OnEnable()
    {
        battleControls.Enable();

        selectedAction = actionButtons[0];

        EventSystem.current.SetSelectedGameObject(selectedAction.gameObject);
        ChangeState(MenuState.ActionMenu);
    }

    private void OnDisable()
    {
        battleControls.Disable();
    }
}

// States of the menu
public enum MenuState
{
    ActionMenu,
    SecondaryMenu
}
