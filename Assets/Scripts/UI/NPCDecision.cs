using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class NPCDecision : MonoBehaviour
{
    [SerializeField] private ScriptableHero player;

    private PlayableDirector _director;
    [SerializeField] private PlayableAsset openingTimeline;
    [SerializeField] private PlayableAsset closingTimeline;

    private bool dialogueFinished = false;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [TextArea(2,3)]
    [SerializeField] private string[] lines;
    [TextArea(2,3)]
    [SerializeField] private string[] linesAfterChoice;
    [SerializeField] private float textSpeed;
    [SerializeField] private GameObject decisionButtons;
    private bool choiceMade = false;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
    }
    private void Start()
    {
        StartCoroutine(DecisionCoroutine());
    }

    IEnumerator DecisionCoroutine()
    {
        _director.Play(openingTimeline);

        while (_director.time < openingTimeline.duration)
        {
            yield return null;
        }

        StartCoroutine(PlayDecisionDialogue(lines));

        while (!dialogueFinished)
        {
            yield return null;
        }

        decisionButtons.SetActive(true);

        while (!choiceMade)
        {
            yield return null;
        }

        decisionButtons.SetActive(false);
        StartCoroutine(PlayDecisionDialogue(linesAfterChoice));

        while (!dialogueFinished)
        {
            yield return null;
        }

        _director.Play(closingTimeline);

        while (_director.time < closingTimeline.duration)
        {
            yield return null;
        }

        SceneManager.LoadScene("OverworldScene");

        yield break;
    }

    IEnumerator PlayDecisionDialogue(string[] linesToPut)
    {
        dialogueFinished = false;

        dialogueText.text = string.Empty;

        for (int i = 0; i < linesToPut.Length; i++)
        {
            foreach (char c in linesToPut[i].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            /*if (i != timeLineCuts[clipIndex].Lines.Length - 1) */dialogueText.text = string.Empty;
        }

        dialogueFinished = true;
    }

    public void SetPlayerMelee(bool choice)
    {
        if (choice) player.Ability = Ability.Melee;
        else player.Ability = Ability.Magic;

        choiceMade = true;
    }
}
