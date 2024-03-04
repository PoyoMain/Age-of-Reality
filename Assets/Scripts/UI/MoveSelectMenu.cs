using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class MoveSelectMenu : MonoBehaviour
{
    private VerticalLayoutGroup vertLayoutGroup;

    [SerializeField] private GameObject attackButton;
    [SerializeField] private GameObject itemButton;
    [SerializeField] private GameObject fleeButton;

    [HideInInspector] public List<MoveOption> moveButtons;
    [HideInInspector] public int moveIndex;

    [HideInInspector] public Ability menuType; // The type of menu this is
    [HideInInspector] public ActionType actionChosen;
    [SerializeField] private int maxElementsOnScreen = 4; // The maximum number of moves that can be displayed at one time
    private int _maxElementsOnScreen;
    private Vector2 elementRange = Vector2.zero;

    [HideInInspector] public HeroUnitBase currentUnit; // The current unit active

    private void Awake()
    {
        vertLayoutGroup = GetComponent<VerticalLayoutGroup>();
        _maxElementsOnScreen = maxElementsOnScreen;
    }

    public void OnEnable()
    {
        DeleteButtons();

        SpawnButtons();
    }

    private void Update()
    {
        if (moveIndex > elementRange.y)
        {
            elementRange += Vector2.one;
            vertLayoutGroup.padding.top -= (int)attackButton.GetComponent<RectTransform>().sizeDelta.y;
            vertLayoutGroup.SetLayoutVertical();
        }
        else if (moveIndex < elementRange.x)
        {
            elementRange -= Vector2.one;
            vertLayoutGroup.padding.top += (int)attackButton.GetComponent<RectTransform>().sizeDelta.y;
            vertLayoutGroup.SetLayoutVertical();
        }
    }

    /// <summary>
    /// Delete existing buttons in menus
    /// </summary>
    private void DeleteButtons()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<MoveOption>().button.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }

        moveButtons.Clear();
        elementRange.Set(0, _maxElementsOnScreen - 1);
        vertLayoutGroup.padding.top = 0;
        vertLayoutGroup.SetLayoutVertical();
    }

    /// <summary>
    /// Dynamically spawn buttons in menu
    /// </summary>
    private void SpawnButtons()
    {
        switch (actionChosen)
        {
            case ActionType.Attack:
                switch (menuType)
                {
                    case Ability.Melee:
                        if (attackButton == null)
                        {
                            Debug.LogError("Attack Button prefab not set in inspector");
                            return;
                        }

                        foreach (ScriptableAttack attack in currentUnit.data.meleeAttacks)
                        {
                            AttackOption moveOption = Instantiate(attackButton, transform).GetComponent<AttackOption>();
                            moveButtons.Add(moveOption);
                            moveOption.Attack = attack;
                            moveOption.gameObject.name = attack.name;
                            moveOption.GetComponentInChildren<TextMeshProUGUI>().text = attack.name;
                        }

                        EventSystem.current.SetSelectedGameObject(moveButtons[0].gameObject);
                        break;
                    case Ability.Magic:
                        if (attackButton == null)
                        {
                            Debug.LogError("Attack Button prefab not set in inspector");
                            return;
                        }

                        foreach (ScriptableAttack attack in currentUnit.data.magicAttacks)
                        {
                            AttackOption moveOption = Instantiate(attackButton, transform).GetComponent<AttackOption>();
                            moveButtons.Add(moveOption);
                            moveOption.Attack = attack;
                            moveOption.gameObject.name = attack.name;
                            moveOption.GetComponentInChildren<TextMeshProUGUI>().text = attack.name;
                        }

                        EventSystem.current.SetSelectedGameObject(moveButtons[0].gameObject);
                        break;
                }
                break;
            case ActionType.Item:
                if (itemButton == null)
                {
                    Debug.LogError("Item Button prefab not set in inspector");
                    return;
                }

                foreach (var item in currentUnit.data.ItemInventory.Inventory)
                {
                    if (currentUnit.data.ItemInventory.HasItem(item.Key))
                    {
                        ItemOption moveOption = Instantiate(itemButton, transform).GetComponent<ItemOption>();
                        moveButtons.Add(moveOption);
                        moveOption.item = item.Value.item;
                        moveOption.gameObject.name = item.Value.item.name;
                        moveOption.GetComponentInChildren<TextMeshProUGUI>().text = item.Value.item.name;
                    }
                }

                EventSystem.current.SetSelectedGameObject(moveButtons[0].gameObject);
                
                break;
            case ActionType.Flee:
                break;
        }
    }
}
