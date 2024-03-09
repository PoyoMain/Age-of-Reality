using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MinigameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _percentDisplayText;
    [SerializeField] private TextMeshProUGUI _timeDisplayText;

    private float timer;

    private LineMinigameBase _minigamePrefab;
    private LineMinigameBase _minigame;

    private Animator anim;

    public bool MinigameRunning { get; private set; }
    private bool isDrawing;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _minigame = Instantiate(_minigamePrefab, GridInfo.GridWorldMidPoint, Quaternion.identity, transform);
        UpdatePercentText();

        isDrawing = true;
    }

    public void SetMinigame(LineMinigameBase minigameToSpawn, int completeTime)
    {
        _minigamePrefab = minigameToSpawn;
        MinigameRunning = true;

        timer = completeTime;
        UpdateTimerText(timer);
    }

    private void Update()
    {
        if (_minigame.StartedDrawing)
        {
            if (timer > 0 && isDrawing)
            {
                timer -= Time.deltaTime;
                UpdateTimerText(timer);
                UpdatePercentText();

                if (timer <= 0 || _minigame.DoneDrawing)
                {
                    if (_minigame.totalPercentage >= 100)
                    {
                        GameManager.Instance.perfectMinigameCount++;
                    }
                    UpdateTimerText(0);
                    _minigame.enabled = false;
                    anim.SetTrigger("MinigameDone");
                    isDrawing = false;
                }
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
