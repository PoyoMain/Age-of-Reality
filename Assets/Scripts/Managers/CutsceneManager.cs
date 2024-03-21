using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector _director;
    [SerializeField] private bool clickToAdvanceScene = false;
    [SerializeField] private int newSceneID;
    [Space(10)]
    [SerializeField] private float timeBetweenCuts = 1;
    [SerializeField] private float timeBetweenLinesSameCut = 0.5f;
    [SerializeField] private float fadeSpeed = 2;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private CanvasGroup cutsceneImage;

    [Space(15)]
    [SerializeField] private CutsceneClip[] timeLineCuts;

    private int clipIndex = 0;
    private bool playNewClip = false;

    private bool dialogueFinished = false;
    private bool videoFinished = false;

    private Coroutine CutsceneVideoCoroutine;
    private Coroutine CutsceneDialogueCoroutine;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();

        StartCoroutine(StartCutscene());
    }

    IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(0.25f);

        _director.Play(timeLineCuts[0].Cut);
        if (timeLineCuts[0].LoopVideo)
        {
            _director.extrapolationMode = DirectorWrapMode.Loop;
        }

        CutsceneVideoCoroutine = StartCoroutine(PlayCutsceneVideo());
        CutsceneDialogueCoroutine = StartCoroutine(PlayCutsceneDialogue());

        yield break;
    }

    IEnumerator PlayCutsceneVideo()
    {
        float fadeAmount = cutsceneImage.alpha;

        while (clipIndex < timeLineCuts.Length)
        {
            if (playNewClip)
            {
                yield return new WaitForSeconds(0.25f);

                playNewClip = false;

                _director.Play(timeLineCuts[clipIndex].Cut);
                CutsceneDialogueCoroutine = StartCoroutine(PlayCutsceneDialogue());

                if (timeLineCuts[clipIndex].LoopVideo) _director.extrapolationMode = DirectorWrapMode.Loop;
                else _director.extrapolationMode = DirectorWrapMode.Hold;
            }

            while (cutsceneImage.alpha < fadeAmount)
            {
                cutsceneImage.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }

            while (_director.time < _director.playableAsset.duration)
            {
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenCuts);

            while (cutsceneImage.alpha > 0)
            {
                cutsceneImage.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }

            videoFinished = true;

            yield return null;
        }

        while (cutsceneImage.alpha > 0)
        {
            cutsceneImage.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        print("Done");
        Invoke(nameof(SceneChange), 1f);
        yield break;
    }

    IEnumerator PlayCutsceneDialogue()
    {
        dialogueText.text = string.Empty;

        for (int i = 0; i < timeLineCuts[clipIndex].Lines.Length; i++)
        {
            foreach (char c in timeLineCuts[clipIndex].Lines[i].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(timeLineCuts[clipIndex].TextSpeed);
            }

            yield return new WaitForSeconds(timeBetweenLinesSameCut);

            if (i != timeLineCuts[clipIndex].Lines.Length - 1) dialogueText.text = string.Empty;
        }

        dialogueFinished = true;
    }

    public void AdvanceDialogue()
    {
        if (dialogueText.text == timeLineCuts[clipIndex].Lines[^1])
        {
            NextShot();
        }
        else
        {
            StopCoroutine(CutsceneDialogueCoroutine);
            dialogueText.text = timeLineCuts[clipIndex].Lines[^1];
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
        if (Input.GetMouseButtonDown(0) && clickToAdvanceScene)
        {
            AdvanceDialogue();
        }

        if (dialogueFinished && videoFinished)
        {
            dialogueFinished = false;
            videoFinished = false;
            NextShot();
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadScene(newSceneID);
    }
}


[Serializable]
public class CutsceneClip
{
    public PlayableAsset Cut;
    [TextArea(2,3)]
    public string[] Lines;
    public float TextSpeed;
    public bool LoopVideo;
}
