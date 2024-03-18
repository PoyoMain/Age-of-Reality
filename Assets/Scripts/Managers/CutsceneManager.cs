using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector _director;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float textSpeed;
    [Space(15)]
    [SerializeField] private CutsceneClip[] timeLineCuts;

    private int clipIndex = 0;
    private bool playNewClip = false;

    private Coroutine CutsceneVideoCoroutine;
    private Coroutine CutsceneDialogueCoroutine;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();

        _director.Play(timeLineCuts[0].Cut);
        if (timeLineCuts[0].Loop)
        {
            _director.extrapolationMode = DirectorWrapMode.Loop;
        }

        CutsceneVideoCoroutine = StartCoroutine(PlayCutsceneVideo());
        CutsceneDialogueCoroutine = StartCoroutine(PlayCutsceneDialogue());
    }

    IEnumerator PlayCutsceneVideo()
    { 
        while (clipIndex < timeLineCuts.Length)
        {
            if (playNewClip)
            {
                _director.Play(timeLineCuts[clipIndex].Cut);
                CutsceneDialogueCoroutine = StartCoroutine(PlayCutsceneDialogue());

                if (timeLineCuts[clipIndex].Loop)
                {
                    _director.extrapolationMode = DirectorWrapMode.Loop;
                }
                else
                {
                    _director.extrapolationMode = DirectorWrapMode.Hold;
                }

                playNewClip = false;
            }

            yield return null;
        }

        print("Done");
        yield break;
    }

    IEnumerator PlayCutsceneDialogue()
    {
        dialogueText.text = string.Empty;

        foreach (char c in timeLineCuts[clipIndex].Text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void AdvanceDialogue()
    {
        if (dialogueText.text == timeLineCuts[clipIndex].Text)
        {
            NextShot();
        }
        else
        {
            StopCoroutine(CutsceneDialogueCoroutine);
            dialogueText.text = timeLineCuts[clipIndex].Text;
        }
    }

    void NextShot()
    {
        if (clipIndex <= timeLineCuts.Length - 1)
        {
            clipIndex++;
            dialogueText.text = string.Empty;
            playNewClip = true;
        }
        else
        {
            StopCoroutine(CutsceneVideoCoroutine);
            StopCoroutine(CutsceneDialogueCoroutine);
            SceneManager.LoadScene("OverworldScene");

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceDialogue();
        }
    }
}


[Serializable]
public class CutsceneClip
{
    public PlayableAsset Cut;
    [TextArea(2,3)]
    public string Text;
    public bool Loop;
}
