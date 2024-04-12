using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class AttackSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject attackButton;

    [HideInInspector] public Ability menuType;

    [HideInInspector] public HeroUnitBase currentUnit; // The current unit active

    private void OnEnable()
    {
        DeleteButtons();

        menuType = currentUnit.data.Ability;

        SpawnButtons();
    }

    private void DeleteButtons()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<MoveOption>().button.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }
    }

    private void SpawnButtons()
    {
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
                    moveOption.Attack = attack;
                    moveOption.gameObject.name = attack.name;
                    moveOption.GetComponentInChildren<TextMeshProUGUI>().text = attack.name + " (MP: " + attack.Stats.MP + ")";
                }

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
                    moveOption.Attack = attack;
                    moveOption.gameObject.name = attack.name;
                    moveOption.GetComponentInChildren<TextMeshProUGUI>().text = attack.name + " (MP: " + attack.Stats.MP + ")";
                }

                break;
        }
    }
    private void OnDisable()
    {
        DeleteButtons();
    }
}
