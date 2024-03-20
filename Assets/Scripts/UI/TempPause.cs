using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TempPause : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject statsMenu;
    public GameObject itemMenu;
    [SerializeField]
    private ScriptableInventory gameInventory;
    public List<PauseItemButton> ButtonsList;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private ScriptableHero Hero;
    [SerializeField] private Ability ability;

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI conText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI stmText;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI atkTypeText;
    [SerializeField] private TextMeshProUGUI statPointText;
    [SerializeField] private GameObject statButtons;
    private int health;
    private int CON;
    private int MaxHP;
    private int ATK;
    private int DEF;
    private int SPD;
    private int STM;
    private int lvl;
    public int statPoints = 0;


    public void Update()
    {
        ability = Hero.Ability;
        health = Hero._stats.Health;
        MaxHP = Hero._stats.Health;
        CON = Hero._stats.Health;
        ATK = Hero._stats.Attack;
        DEF = Hero._stats.Defense;
        SPD = Hero._stats.Speed;
        STM = Hero._stats.Stamina;
        lvl = Hero._stats.Level;
        SetText();

        if (statPoints > 0)
        {
            statButtons.SetActive(true);
        }
        else
        {
            statButtons.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause_Game();
            }
        }
    }
    public void ChangeScene(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    public void Quit_Game()
    {
        Application.Quit();
    }

    public void Pause_Game()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        stats();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void stats()
    {
        statsMenu.SetActive(true);
        itemMenu.SetActive(false);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void items()
    {
        statsMenu.SetActive(false);
        itemMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        read();
    }


    public void read()
    {
        for (int i = 0; i < GameManager.Instance.ItemInventory.Inventory.Count; i++)
        {
            if (gameInventory.Inventory[i].Value.Amount > 0)
            {
                ButtonsList[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Instance.ItemInventory.Inventory[i].Value.item.name;
                ButtonsList[i].item = GameManager.Instance.ItemInventory.Inventory[i].Value;
            }
        }
    }

    public void changeText(int index)
    {
        itemNameText.text = ButtonsList[index].item.item.name;
        itemDescriptionText.text = ButtonsList[index].item.item.Description;
        itemSprite.sprite = ButtonsList[index].item.item.Sprite;


    }

    public void SetText()
    {

        lvlText.text = "Lvl: " + lvl.ToString();
        if (health == MaxHP)
        {
            hpText.text = "Health: " + health.ToString();
        }
        else
        {
            hpText.text = "Health: " + health.ToString() + "/" + MaxHP.ToString();
        }

        switch (ability)
        {
            case Ability.Magic:

                atkText.text = "INT: " + ATK.ToString();
                stmText.text = "WIS: " + STM.ToString();
                break;
            case Ability.Melee:
                atkText.text = "ATK: " + ATK.ToString();
                stmText.text = "STM: " + STM.ToString();
                break;
        }
        atkTypeText.text = ability.ToString();
        statPointText.text = "Stat Points: " + statPoints.ToString();
        conText.text = "CON: " + CON.ToString();
        spdText.text = "SPD: " + SPD.ToString();
        defText.text = "DEF: " + DEF.ToString();

    }

    public void points()
    {
        statPoints--;
    }

    public void HpUP()
    {
        Hero._stats.Health++;
    }

    public void AtkUP()
    {
        Hero._stats.Attack++;
    }

    public void DefUP()
    {
        Hero._stats.Defense++;
    }

    public void SpdUP()
    {
        Hero._stats.Speed++;
    }

    public void StmUP()
    {
        Hero._stats.Stamina++;
    }
}
