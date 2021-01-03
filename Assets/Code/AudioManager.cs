using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] m_Sounds;
    // Start is called before the first frame update
    void Awake ()
    {
        foreach(Sound s in m_Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else
            Debug.Log("null here");
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.name == name);
        if (s != null)
        {
            if (s.source.isPlaying)
                return true;
        }//spartial bland for 2d/3d
        return false;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
    }

    public void ChangeSpeed(string name)
    {
        Debug.Log("changing speeddddd");
        Sound s = Array.Find(m_Sounds, sound => sound.name == name);
        float newSpeed = 0.5f;
        s.source.pitch = newSpeed;
        s.source.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / newSpeed);
    }

    public void PlayS(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.name == name);
        s.source.PlayScheduled(4);
      //  s.source.SetScheduledEndTime(7);
    }
}
