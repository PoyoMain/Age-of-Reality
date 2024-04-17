using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialogueBox;
    private DialogueLine line;
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private AudioSource mainMusicAudioSource;
    private Coroutine textCoroutine;
    private bool dialogueBoxOpen;
    //private int index;
    private float startMainVolume;

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

        nameTxt.text = unit.name;
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
        if (textCoroutine == null) return;
        StopCoroutine(textCoroutine);

        if (textComponent.text == line.Line)
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = line.Line;

            if (dialogueAudioSource != null) dialogueAudioSource.Stop();
            mainMusicAudioSource.volume = startMainVolume;
        }
    }

    void StartDialogue()
    {
        //index = 0;
        //dialogueDone = false;

        if (!(dialogueAudioSource == null || line.VoiceClips == null))
        {
            if (line.VoiceClips.Length > 0)
            {
                dialogueAudioSource.clip = line.VoiceClips[Random.Range(0, line.VoiceClips.Length)];
                dialogueAudioSource.Play();
                startMainVolume = mainMusicAudioSource.volume;
                mainMusicAudioSource.volume /= 2;
            }
        }

        textCoroutine = StartCoroutine(TypeLine());
        Debug.Log("Start Dialogue");
    }

    IEnumerator TypeLine()
    {
        foreach (char c in line.Line.ToCharArray())
        {
            if (textComponent.text == line.Line) yield break;

            textComponent.text += c;
            yield return new WaitForSeconds(line.textSpeed);
        }

        //dialogueDone = true;
        yield break;
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
            textCoroutine = null;
            dialogueBox.SetActive(false);
            PlayingDialogue = false;

        //}
    }
}