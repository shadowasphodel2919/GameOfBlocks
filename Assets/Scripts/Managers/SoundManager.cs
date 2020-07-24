using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool m_musicEnabled = true;
    public bool m_fxEnabled = true;

    [Range(0, 1)]
    public float m_musicVolume = 1.0f;

    [Range(0, 1)]
    public float m_fxVolume = 1.0f;

    public AudioClip m_clearRowSound;
    public AudioClip m_moveSound;
    public AudioClip m_dropSound;
    public AudioClip m_gameOverSound;
    public AudioClip m_backgroundMusic;//unused
    public AudioSource m_musicSource;
    public AudioClip m_errorSound;

    public AudioClip[] m_exclaimationSounds;
    public AudioClip m_gameOverVocal;
    public AudioClip m_levelUpVocal;

    public AudioClip[] m_audioClips;//INVOKE THIS FOR RANDOM CLIPS
    public AudioClip m_randomClip;
    public AudioClip returnRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }

    public IconToggle m_musicIconToggle;
    public IconToggle m_fxIconToggle;

    public AudioClip m_holdSound;

    void Start()
    {
        playBackgroundMusic(returnRandomClip(m_audioClips));
    }

    void Update()
    {
        
    }

    public void playBackgroundMusic(AudioClip musicClip)
    {
        if (!m_musicEnabled || !musicClip || !m_musicSource)
        {
            return;
        }
        m_musicSource.Stop();
        m_musicSource.clip = musicClip;
        m_musicSource.volume = m_musicVolume;
        m_musicSource.loop = true;
        m_musicSource.Play();
    }

    void updateMusic()
    {
        if (m_musicSource.isPlaying != m_musicEnabled)
        {
            if (m_musicEnabled)
            {
                playBackgroundMusic(returnRandomClip(m_audioClips));
            }
            else
            {
                m_musicSource.Stop();
            }
        }
    }

    public void toggleMusic()
    {
        m_musicEnabled = !m_musicEnabled;
        updateMusic();

        if (m_musicIconToggle)
        {
            m_musicIconToggle.iconToggle(m_musicEnabled);
        }
    }

    public void toggleFX()
    {
        m_fxEnabled = !m_fxEnabled;
        if (m_fxIconToggle)
        {
            m_fxIconToggle.iconToggle(m_fxEnabled);
        }
    }
}
