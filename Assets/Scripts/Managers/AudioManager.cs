using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource battleAudioSource;
    [Space(20)]
    [SerializeField] private AudioClip[] playerSwordAudioClips;
    [SerializeField] private AudioClip[] playerFireAudioClips;
    [SerializeField] private AudioClip[] playerHurtAudioClips;
    [SerializeField] private AudioClip[] enemyAttackAudioClips;
    [SerializeField] private AudioClip[] enemyHurtAudioClips;
    [SerializeField] private AudioClip battleItemJingle;
    [SerializeField] private AudioClip battleRareItemJingle;
    [SerializeField] private AudioClip battleLevelUpJingle;



    public void PlayPlayerSwingSFX()
    {
        if (playerSwordAudioClips == null || playerSwordAudioClips.Length < 0)
        {
            Debug.LogError("No Player Sword Audio Clips in Audio Manager");
            return;
        }

        if (battleAudioSource == null)
        {
            Debug.LogError("Battle Audio Source not set in Audio Manager");
            return;
        }

        AudioClip clipToPlay = playerSwordAudioClips[Random.Range(0, playerSwordAudioClips.Length)];
        battleAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayPlayerFireSFX()
    {
        if (playerFireAudioClips == null || playerFireAudioClips.Length < 0)
        {
            Debug.LogError("No Player Fire Audio Clips in Audio Manager");
            return;
        }

        if (battleAudioSource == null)
        {
            Debug.LogError("Battle Audio Source not set in Audio Manager");
            return;
        }

        AudioClip clipToPlay = playerFireAudioClips[Random.Range(0, playerFireAudioClips.Length)];
        battleAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayPlayerHurtSFX()
    {
        if (playerHurtAudioClips == null || playerHurtAudioClips.Length < 0)
        {
            Debug.LogError("No Player Sword Audio Clips in Audio Manager");
            return;
        }

        if (battleAudioSource == null)
        {
            Debug.LogError("Battle Audio Source not set in Audio Manager");
            return;
        }

        AudioClip clipToPlay = playerHurtAudioClips[Random.Range(0, playerHurtAudioClips.Length)];
        battleAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayEnemySwingSFX()
    {
        if (enemyAttackAudioClips == null || enemyAttackAudioClips.Length < 0)
        {
            Debug.LogError("No Enemy Attack Audio Clips in Audio Manager");
            return;
        }

        if (battleAudioSource == null)
        {
            Debug.LogError("Battle Audio Source not set in Audio Manager");
            return;
        }

        AudioClip clipToPlay = enemyAttackAudioClips[Random.Range(0, enemyAttackAudioClips.Length)];
        battleAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayEnemyHurtSFX()
    {
        if (enemyHurtAudioClips == null || enemyHurtAudioClips.Length < 0)
        {
            Debug.LogError("No Enemy Hurt Audio Clips in Audio Manager");
            return;
        }

        if (battleAudioSource == null)
        {
            Debug.LogError("Battle Audio Source not set in Audio Manager");
            return;
        }

        AudioClip clipToPlay = enemyHurtAudioClips[Random.Range(0, enemyHurtAudioClips.Length)];
        battleAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayBattleSFX(AudioClip clip = null, BattleSounds soundType = BattleSounds.None)
    {
        if (battleAudioSource == null)
        {
            Debug.LogError("Battle Audio Source not set in Audio Manager");
            return;
        }

        switch (soundType)
        {
            case BattleSounds.None:
                if (clip == null)
                {
                    Debug.LogError("No clip set");
                    return;
                }
                battleAudioSource.PlayOneShot(clip);
                break;
            case BattleSounds.LevelUp:
                battleAudioSource.PlayOneShot(battleLevelUpJingle);
                break;
            case BattleSounds.Item:
                battleAudioSource.PlayOneShot(battleItemJingle);
                break;
            case BattleSounds.RareItem:
                battleAudioSource.PlayOneShot(battleRareItemJingle);
                break;
        }
    }


    //public void CrossFadeAudio(AudioSource source, AudioClip newClipToPlay, float fadeTime)
    //{
    //    StartCoroutine(CrossFadeAudioCoroutine(source, newClipToPlay, fadeTime));
    //}

    //private IEnumerator CrossFadeAudioCoroutine(AudioSource source, AudioClip newClipToPlay, float fadeTime)
    //{
    //    float startVolume = source.volume;

    //    while (source.volume > 0)
    //    {
    //        source.volume -= startVolume * Time.deltaTime / fadeTime;
    //        yield return null;
    //    }

    //    source.clip = newClipToPlay;

    //    while (source.volume <  startVolume)
    //    {
    //        source.volume += startVolume * Time.deltaTime / fadeTime;
    //        yield return null;
    //    }
    //}
}

public enum BattleSounds
{
    None,
    LevelUp,
    Item,
    RareItem,
}
