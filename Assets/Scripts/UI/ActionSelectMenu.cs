using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class ActionSelectMenu : MonoBehaviour
{
    [SerializeField] private MoveSelectMenu moveSelectMenu; // The sub menu to select moves
    [SerializeField] private AttackSelectMenu attackSelectMenu;
    [SerializeField] private ItemSelectMenu itemSelectMenu;

    [HideInInspector] public ScriptableAttack chosenAttack = null; // The attack chosen from the move select menu
    [HideInInspector] public ScriptableItem chosenItem = null;
    private MoveOption selectedMove;
    private List<MoveOption> moveButtons;
    private int moveIndex;

    private PlayerControls playerControls;
    private PlayerControls.BattleControlsActions battleControls;

    private MenuState menuState;

    [HideInInspector] public bool flee = false;

    private Animator _anim;

    private void Awake()
    {
        playerControls = new PlayerControls();
        battleControls = playerControls.BattleControls;

        //moveButtons = moveSelectMenu.moveButtons;
        //moveIndex = moveSelectMenu.moveIndex;

        _anim = GetComponent<Animator>();
        _anim.keepAnimatorStateOnDisable = false;
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
                attackSelectMenu.enabled = false;
                itemSelectMenu.enabled = false;
                break;
            case MenuState.AttackMenu:
                attackSelectMenu.enabled = true;
                break;
            case MenuState.ItemMenu:
                itemSelectMenu.enabled = true;
                break;
        }
    }

    public void SelectAttackOption()
    {
        StartCoroutine(SelectAttack());
    }

    IEnumerator SelectAttack()
    {
        //moveSelectMenu.menuType = moveSelectMenu.currentUnit.data.Ability;
        //moveSelectMenu.actionChosen = ActionType.Attack;
        //_anim.SetTrigger("ShowAttacks");

        if (_anim.GetBool("isAttackMenu"))
        {
            _anim.SetBool("isAttackMenu", false);
            ChangeState(MenuState.ActionMenu);
        }
        else
        {
            _anim.SetTrigger("AttackMenuOpen");

            yield return 0;

            _anim.SetBool("isAttackMenu", true);
            _anim.SetBool("isItemMenu", false);

            yield return new WaitForSeconds(0.25f);

            ChangeState(MenuState.AttackMenu);
        }

        yield break;
    }

    public void SelectItemOption()
    {
        StartCoroutine(SelectItem());
    }

    IEnumerator SelectItem()
    {
        //moveSelectMenu.menuType = moveSelectMenu.currentUnit.data.Ability;
        //moveSelectMenu.actionChosen = ActionType.Attack;
        //_anim.SetTrigger("ShowItems");

        if (_anim.GetBool("isItemMenu"))
        {
            _anim.SetBool("isItemMenu", false);
            ChangeState(MenuState.ActionMenu);
        }
        else
        {
            _anim.SetTrigger("ItemMenuOpen");

            yield return 0;

            _anim.SetBool("isItemMenu", true);
            _anim.SetBool("isAttackMenu", false);

            yield return new WaitForSeconds(0.25f);

            ChangeState(MenuState.ItemMenu);
        }
        
        yield break;
    }

    public void SelectFleeOption()
    {
        flee = true;
    }

    public void ResetAnimator()
    {
        StartCoroutine(ResetCoroutine());
    }

    private IEnumerator ResetCoroutine()
    {
        _anim.SetBool("isAttackMenu", false);
        _anim.SetBool("isItemMenu", false);
        _anim.SetTrigger("CloseFullMenu");

        yield return new WaitForSeconds(1);

        flee = false;
    }

    public void MoveSelected(MoveOption option)
    {
        StartCoroutine(MoveSelect(option));
    }

    IEnumerator MoveSelect(MoveOption option)
    {
        if (option is AttackOption attackOption)
        {
            _anim.SetBool("isAttackMenu", false);

            yield return new WaitForSeconds(1);

            chosenAttack = attackOption.Attack;
            ChangeState(MenuState.ActionMenu);
        }
        else if (option is ItemOption itemOption)
        {
            _anim.SetBool("isItemMenu", false);

            yield return new WaitForSeconds(1);

            chosenItem = itemOption.item;
            ChangeState(MenuState.ActionMenu);
        }
    }

    public void SetCurrentUnit(HeroUnitBase hero)
    {
        //moveSelectMenu.currentUnit = hero;
        attackSelectMenu.currentUnit = hero;
        itemSelectMenu.currentUnit = hero;
    }

    private void OnEnable()
    {
        battleControls.Enable();
        ChangeState(MenuState.ActionMenu);
    }

    private void OnDisable()
    {
        _anim.SetBool("isAttackMenu", false);
        _anim.SetBool("isItemMenu", false);
        _anim.SetTrigger("CloseFullMenu");
        battleControls.Disable();
    }

    //public void selectButton(int index)
    //{
    //    selectedAction = actionButtons[index];
    //    EventSystem.current.SetSelectedGameObject(selectedAction.gameObject);
    //    Select();
    //}
}

// States of the menu
public enum MenuState
{
    ActionMenu,
    AttackMenu,
    ItemMenu,
}