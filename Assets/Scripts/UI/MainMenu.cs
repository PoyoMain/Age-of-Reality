using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject mainMenu;
    //Plays the game; advance to the main scene
    public void PlayGame()
    {
        SceneManager.LoadScene("OverworldScene");
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Swaps to the options screen from main menu
    public void OptionsButton()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    //Swaps to main menu from options screen
    public void BackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
