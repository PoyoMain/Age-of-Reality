using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayableDirector))]
public class CreditsManager : MonoBehaviour
{
    private PlayableDirector _director;
    [SerializeField] private SkimButton skimButton;
    [SerializeField] private float speedUpSpeed;
    [SerializeField] private int buttonDisappearTime;
    [SerializeField] private AudioSource creditsAudioSource;
    private float timeSinceLastInput;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            timeSinceLastInput = 0;
            skimButton.gameObject.SetActive(true);
        }
        else
        {
            timeSinceLastInput += Time.deltaTime;

            if (skimButton.gameObject.activeSelf && timeSinceLastInput >= buttonDisappearTime)
            {
                skimButton.gameObject.SetActive(false);
            }
        }

        if (skimButton.buttonPressed)
        {
            _director.playableGraph.GetRootPlayable(0).SetSpeed(speedUpSpeed);
            timeSinceLastInput = 0;
            creditsAudioSource.pitch = speedUpSpeed;

        }
        else
        {
            _director.playableGraph.GetRootPlayable(0).SetSpeed(1);
            creditsAudioSource.pitch = 1;
        }


        if (_director.time >= _director.duration)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
