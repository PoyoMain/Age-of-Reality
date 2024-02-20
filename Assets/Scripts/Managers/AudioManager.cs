using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public void PlayAudio(AudioClip clip, AudioSource source)
    {
            
    }

    public void CrossFadeAudio(AudioSource source, AudioClip newClipToPlay, float fadeTime)
    {
        StartCoroutine(CrossFadeAudioCoroutine(source, newClipToPlay, fadeTime));
    }

    private IEnumerator CrossFadeAudioCoroutine(AudioSource source, AudioClip newClipToPlay, float fadeTime)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        source.clip = newClipToPlay;

        while (source.volume <  startVolume)
        {
            source.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
