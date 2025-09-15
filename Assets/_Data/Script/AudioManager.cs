using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : BaseManager<AudioManager>
{

    public AudioSource musicSource, sfxSource;
    public Sound[] musicSounds, sfxSounds;

    private Dictionary<string, AudioSource> sfxSources = new Dictionary<string, AudioSource>();
    protected override void Awake()
    {
        base.Awake();
        foreach (Sound s in sfxSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.playOnAwake = false;
            sfxSources[s.name] = source;
        }
    }

    public void PlayBGM(string name)
    {
        Sound s = System.Array.Find(musicSounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        AudioSource source = sfxSources[name];
        source.pitch = Random.Range(0.8f, 1.0f);
        source.Play();
    }

    public void StopSFX(string name)
    {
        if (!sfxSources.ContainsKey(name)) return;
        sfxSources[name].Stop();
    }

    public bool IsSFXPlaying(string name)
    {
        return sfxSources.ContainsKey(name) && sfxSources[name].isPlaying;
    }
    public void ToggleBGM()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void BGMVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}
