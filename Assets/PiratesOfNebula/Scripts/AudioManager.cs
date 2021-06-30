using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class Sound
{
    public string Name;
    public enum SoundType { Normal, Music };
    public enum Activation {Custom,Shoot,ParticleSpawn,PlayInstantly};
    public SoundType soundType;
    public Activation activation;
    public AudioClip Clip;
    public bool Loop;
    public bool HearEveryWhere;
    public float MinRange = 20;
    public float MaxRange = 30;
    [Range(0f, 1f)]
    public float Volume = 1f;
    [Range(.1f, 3f)]
    public float Pitch = 1f;
    [Range(0f,2f)]
    public float RandomizedPitch;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    AudioMixerGroup[] AudioMixers;
    
    public Sound[] Sounds;

    private void Awake()
    {
       
    }

    

    // Start is called before the first frame update
    void Start()
    {
        AudioMixers = Settings.audiomixergroup;

        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.loop = s.Loop;
            if (!s.HearEveryWhere)
            {
                s.source.spatialBlend = 1;
                s.source.rolloffMode = AudioRolloffMode.Linear;
                s.source.minDistance = s.MinRange;
                s.source.maxDistance = s.MaxRange;
            }
            if (s.soundType == Sound.SoundType.Normal)
            {
                s.source.outputAudioMixerGroup = AudioMixers[0];
            }
            else if (s.soundType == Sound.SoundType.Music)
            {
                s.source.outputAudioMixerGroup = AudioMixers[1];
            }
            if (s.activation == Sound.Activation.PlayInstantly)
            {
                PlaySound(s);
            }
        }
    }
    public void PlaySound(Sound.Activation A, string Name = "")
    {
        if (A == Sound.Activation.Custom)
        {
            if (Name != "")
            {
                Sound s = Array.Find(Sounds, S => S.Name == Name);
                if (s == null)
                {
                    Debug.LogWarning($"The Sound '{Name}' not found");
                    return;
                }
                PlaySound(s);
            }
            else Debug.LogWarning($"Custom sounds must be called with a name");
        }
        else
        {
            foreach (Sound.Activation a in (Sound.Activation[])Enum.GetValues(typeof(Sound.Activation)))
            {
                if (A == a)
                {
                    foreach (Sound s in Sounds)
                    {
                        if (s.activation == a)
                        {
                            PlaySound(s);
                        }
                    }
                    break;
                }
            }
        }
    }
    void PlaySound(Sound s)
    {
        if (s.RandomizedPitch > 0)
        {
            float minPitch = s.Pitch - s.RandomizedPitch;
            if (minPitch < 0.1f) minPitch = 0.1f;
            float maxPitch = s.Pitch + s.RandomizedPitch;
            if (maxPitch > 3) maxPitch = 3;
            s.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        }
        else s.source.pitch = s.Pitch;
        s.source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
