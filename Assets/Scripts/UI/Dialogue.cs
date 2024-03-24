using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialogueBox;
    private string[] lines;
    [SerializeField] private float textSpeed;
    private int index;

    public bool PlayingDialogue
    {
        get;
        private set;
    }

    public void Init(string[] linesToSay)
    {
        if (linesToSay == null) return;

        if (linesToSay.Length == 0) return;

        lines = linesToSay;
        dialogueBox.SetActive(true);
        textComponent.text = string.Empty;
        PlayingDialogue = true;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PlayingDialogue)
        {
            AdvanceDialogue();
        }
    }

    public void AdvanceDialogue()
    {
        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            PlayingDialogue = false;

        }
    }
}