using UnityEngine;
using UnityEngine.Audio;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetMusicVolume(float value)
    {
        musicMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        sfxMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }
}
