using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBG : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClipsMain;
    [SerializeField] private List<AudioClip> audioClipsAlt;

    private List<AudioClip> audioClipsToPlay;

    private AudioSource _audioSource;

    [SerializeField] private float audioSwapTime;
    private float _timer;

    [SerializeField] private float fadeTime;

    [HideInInspector] public bool playAltTracks;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        if (!playAltTracks)
        {
            if (audioClipsMain == null) return;
            audioClipsToPlay = audioClipsMain;
        }
        else
        {
            if (audioClipsAlt == null) return;
            audioClipsToPlay = audioClipsAlt;
        }
        

        StartCoroutine(PlayBGAudio());
    }

    private IEnumerator PlayBGAudio()
    {
        int index = 0;
        float startVolume = _audioSource.volume;

        _audioSource.clip = audioClipsToPlay[0];
        _audioSource.Play();
        _audioSource.volume = 0;
        _timer = audioSwapTime;

        while (_audioSource.volume < startVolume)
        {
            _audioSource.volume += startVolume * Time.deltaTime / fadeTime; 
            yield return null;
        }

        if (audioSwapTime == 0) yield break;

        while (this.enabled)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                while (_audioSource.volume > 0)
                {
                    _audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                    yield return null;
                }

                index = (index >= audioClipsToPlay.Count - 1) ? 0 : index + 1;
                _audioSource.clip = audioClipsToPlay[index];
                _audioSource.Play();
                _timer = audioSwapTime;

                while (_audioSource.volume < startVolume)
                {
                    _audioSource.volume += startVolume * Time.deltaTime / fadeTime;
                    yield return null;
                }

                
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.modeTransitioning)
        {
            StopCoroutine(PlayBGAudio());
        }
    }
}
