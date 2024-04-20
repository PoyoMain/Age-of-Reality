using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayableDirector))]
public class NPCDecision : MonoBehaviour
{
    [SerializeField] private ScriptableHero player;

    private PlayableDirector _director;
    [SerializeField] private PlayableAsset openingTimeline;
    [SerializeField] private PlayableAsset closingTimeline;

    [SerializeField] private Sprite playerImage;
    [SerializeField] private Sprite NPCImage;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerName;

    private bool dialogueFinished = false;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [TextArea(2,3)]
    [SerializeField] private string[] lines;
    [TextArea(2, 3)]
    [SerializeField] private string[] linesAfterDecision;
    [TextArea(2,3)]
    [SerializeField] private string[] linesAfterChoice;
    [SerializeField] private float textSpeed;
    [SerializeField] private GameObject decisionButtons;
    private bool choiceMade = false;
    private bool skipDialogue = false;

    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Sprite meleeInstructions;
    [SerializeField] private Sprite magicInstructions;

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
        StartCoroutine(PlayDecisionDialogue(linesAfterDecision));

        while (!dialogueFinished)
        {
            yield return null;
        }

        ShowInstructions(player.Ability == Ability.Magic);

        while (instructionsPanel.activeInHierarchy)
        {
            yield return null;
        }

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

        bool readingName = false;
        string nameString = "";

        for (int i = 0; i < linesToPut.Length; i++)
        {
            foreach (char c in linesToPut[i].ToCharArray())
            {
                if (skipDialogue)
                {
                    skipDialogue = false;
                    dialogueText.text = string.Empty;
                    bool skipName = false;
                    foreach (char c2 in linesToPut[i])
                    {
                        if (c2 == '<')
                        {
                            skipName = true;
                        }
                        if (c2 == '>')
                        {
                            skipName = false;
                        }

                        if (!skipName && c2 != '>')
                        {
                            dialogueText.text += c2;
                        }
                        
                    }
                    dialogueText.text = linesToPut[i];
                    break;
                }
                if (c == '<') readingName = true;
                else if (c == '>')
                {
                    readingName = false;

                    if (nameString == "Player")
                    {
                        speakerImage.sprite = playerImage;
                        speakerName.text = "Player";
                    }
                    else if (nameString == "NPC")
                    {
                        speakerImage.sprite = NPCImage;
                        speakerName.text = "NPC";
                    }

                    nameString = string.Empty;
                }

                if (readingName && c != '<' && c != '>') nameString += c;
                else if (!readingName)
                {
                    dialogueText.text += c;
                    yield return new WaitForSeconds(textSpeed);
                }
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
        player.ResetCharacter();

        if (choice) player.Ability = Ability.Melee;
        else player.Ability = Ability.Magic;

        choiceMade = true;
    }

    public void ShowInstructions(bool magic)
    {
        instructionsPanel.GetComponent<Image>().sprite = magic ? magicInstructions : meleeInstructions;
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipDialogue = true;
        }
    }
}
