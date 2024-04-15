using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Number : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI Hint;
    public TextMeshProUGUI Health;
    public string[] hints;
    public int bar = 11;
    public int randomNumber;
    public int user;
    public bool lose = false;
    public bool win = false;
    public int turn = 10;
    public static int won = 0;
    void Start()
    {
        randomNumbers();
        Hint.text = "Hint: " + hints[randomNumber];
    }

    // Update is called once per frame
    void Update()
    {
        Health.text = "Health: " + bar;
        if(lose){
            Health.text = "You failed. You are not worthy to fight me!!! Goodbye!!!";
            Application.Quit();
        }
        if(win){
            Health.text = "You won. I am not worthy to fight you!!! Nooooo!!!";
            won++;
            PlayerPrefs.SetInt("Medal",won);
            won = PlayerPrefs.GetInt("Medal");
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
    }

    public void click(int u){
        user = u;
        if(user == randomNumber){
            damage();
        } else {
            lose = true;
        }
        if(turn > 0){
            randomNumbers();
            Hint.text = "Hint: " + hints[randomNumber];
            turn--;
        } else {
            win = true;
        }
    }

    public void damage(){
        bar--;
    }

    public void randomNumbers(){
        randomNumber = Random.Range(0, 10);
        Debug.Log(randomNumber);
    }
}
