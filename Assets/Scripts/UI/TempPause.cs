using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TempPause : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject statsMenu;
    public GameObject itemMenu;
    [SerializeField]
    private ScriptableInventory gameInventory;
    public List <Button> ButtonsList;
    public void Update()
    {
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
            }
        }
    }
}
