using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MinigameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _percentDisplayText;
    [SerializeField] private TextMeshProUGUI _timeDisplayText;

    [SerializeField] private float minigameCompleteTime;
    private float timer;

    private LineGenerator _minigamePrefab;
    private LineGenerator _minigame;

    private Animator anim;

    public bool MinigameRunning { get; private set; }
    private bool isDrawing;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        timer = minigameCompleteTime;
        UpdateTimerText(timer);

        _minigame = Instantiate(_minigamePrefab, GridInfo.GridWorldMidPoint, Quaternion.identity, transform);
        UpdatePercentText();

        isDrawing = true;
    }

    public void SetMinigame(LineGenerator minigameToSpawn)
    {
        _minigamePrefab = minigameToSpawn;
        MinigameRunning = true;
    }

    private void Update()
    {
        
        if (timer > 0 && isDrawing)
        {
            timer -= Time.deltaTime;
            UpdateTimerText(timer);
            UpdatePercentText();

            if (timer <= 0 || _minigame.DoneDrawing)
            {
                UpdateTimerText(0);
                _minigame.enabled = false;
                anim.SetTrigger("MinigameDone");
                isDrawing = false;
            }
        }
    }

    void UpdateTimerText(float newTime)
    {
        _timeDisplayText.text = newTime.ToString("F2");
    }

    void UpdatePercentText()
    {
        _percentDisplayText.text = _minigame.totalPercentage.ToString() + "%";
    }

    public void EndMinigame()
    {
        Destroy(_minigame.gameObject);
        MinigameRunning = false;
    }
}
