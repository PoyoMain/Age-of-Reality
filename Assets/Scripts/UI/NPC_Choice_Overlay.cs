using UnityEngine;

public class NPC_Choice_Overlay : MonoBehaviour
{
    public GameObject choiceUI;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            choiceUI.SetActive(true);
        }
    }

    public void buttonClick()
    {
        choiceUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
