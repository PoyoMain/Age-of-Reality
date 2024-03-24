using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class volume_script : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume (float Volume)
    {
        audioMixer.SetFloat("masterVol", Volume);
    }
}
