using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {

    public AudioMixer mixer;

    public void SetVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", volume);
    }
	
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
