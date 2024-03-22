using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Dialogue : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialogueBox;
    private DialogueLine line;
    [SerializeField] private AudioSource audioSource;
    //private int index;

    private Animator _anim;

    public bool PlayingDialogue
    {
        get;
        private set;
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Init(DialogueLine lineToSay, ScriptableUnitBase unit)
    {
        if (lineToSay == null) return;

        //if (lineToSay.Length == 0) return;

        line = lineToSay;
        dialogueBox.SetActive(true);
        textComponent.text = string.Empty;
        PlayingDialogue = true;

        _anim.SetTrigger("Open");
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
        if (textComponent.text == line.Line)
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = line.Line;
        }
    }

    void StartDialogue()
    {
        //index = 0;

        if (!(audioSource == null || line.VoiceClips == null))
        {
            if (line.VoiceClips.Length > 0)
            {
                audioSource.PlayOneShot(line.VoiceClips[Random.Range(0, line.VoiceClips.Length)]);
            }
        }

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in line.Line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(line.textSpeed);
        }
    }

    void NextLine()
    {
        //if (index < line.Length - 1)
        //{
        //    index++;
        //    textComponent.text = string.Empty;
        //    StartCoroutine(TypeLine());
        //}
        //else
        //{

            _anim.SetTrigger("Close");
            dialogueBox.SetActive(false);
            PlayingDialogue = false;

        //}
    }
}