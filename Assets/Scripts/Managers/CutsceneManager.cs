using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector _director;
    [SerializeField] private bool clickToAdvanceScene = false;
    [Space(10)]
    [SerializeField] private float timeBetweenCuts = 1;
    [SerializeField] private float fadeSpeed = 2;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private CanvasGroup cutsceneImage;

    [Space(15)]
    [SerializeField] private CutsceneClip[] timeLineCuts;

    private int clipIndex = 0;
    private bool playNewClip = false;
    public int SceneID = 0;

    private Coroutine CutsceneVideoCoroutine;
    private Coroutine CutsceneDialogueCoroutine;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();

        _director.Play(timeLineCuts[0].Cut);
        if (timeLineCuts[0].LoopVideo)
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
                float fadeAmount = cutsceneImage.alpha;
                print(cutsceneImage.alpha);

                while (cutsceneImage.alpha > 0)
                {
                    print(cutsceneImage.alpha);
                    cutsceneImage.alpha -= fadeSpeed * Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(timeBetweenCuts / 2);
                _director.Play(timeLineCuts[clipIndex].Cut);
                yield return new WaitForSeconds(timeBetweenCuts / 2);
                CutsceneDialogueCoroutine = StartCoroutine(PlayCutsceneDialogue());

                if (timeLineCuts[clipIndex].LoopVideo)
                {
                    _director.extrapolationMode = DirectorWrapMode.Loop;
                }
                else
                {
                    _director.extrapolationMode = DirectorWrapMode.Hold;
                }

                playNewClip = false;

                while (cutsceneImage.alpha < fadeAmount)
                {
                    cutsceneImage.alpha += fadeSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            yield return null;
        }

        print("Done");
        Invoke("SceneLoad", 1f);

        yield break;
    }

    IEnumerator PlayCutsceneDialogue()
    {
        dialogueText.text = string.Empty;

        foreach (char c in timeLineCuts[clipIndex].Text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(timeLineCuts[clipIndex].TextSpeed);
        }

        NextShot();
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
        if (Input.GetMouseButtonDown(0) && clickToAdvanceScene)
        {
            AdvanceDialogue();
        }
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(SceneID);
    }
}


[Serializable]
public class CutsceneClip
{
    public PlayableAsset Cut;
    [TextArea(2, 3)]
    public string Text;
    public float TextSpeed;
    public bool LoopVideo;
}
