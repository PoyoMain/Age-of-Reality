using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class volume_script : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume (float masterVolume)
    {
        audioMixer.SetFloat("masterVol", masterVolume);
    }

    public void SetSFX (float SFX)
    {
        audioMixer.SetFloat("SFX", SFX);
    }

    public void SetMusic (float Music)
    {
        audioMixer.SetFloat("Music", Music);
    }

    public void SetDialogueVolume (float Dialogue)
    {
        audioMixer.SetFloat("Dialogue", Dialogue);
    }
}
