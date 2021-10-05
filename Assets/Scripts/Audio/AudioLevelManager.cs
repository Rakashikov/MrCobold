using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioLevelManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sound[] sounds;

    private float currentVolume;
    private float startVolume;

    private void Awake()
    {
        audioMixer.GetFloat("MasterVolume", out startVolume);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Start()
    {
        Play("NormAmbient");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void StopPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void ChangeVolume(float value)
    {
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        audioMixer.SetFloat("MasterVolume",  currentVolume + ((80 - Mathf.Abs(startVolume)) / 100 * value));
    }

    public void ChangePitch(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.pitch = value;
    }
}
