using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource voiceSource;

    [Header("SFX")]
    public AudioClip hit;
    public AudioClip ko;
    // public AudioClip block;   
    // public AudioClip whiff;   

    [Header("Announcer")]
    public AudioClip preRoundVoice;
    public AudioClip roundStartVoice;
    public AudioClip koVoice;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHit()
    {
        if (hit != null && sfxSource != null)
            sfxSource.PlayOneShot(hit);
    }

    public void PlayKO()
    {
        if (ko != null && sfxSource != null)
            sfxSource.PlayOneShot(ko);

        if (koVoice != null && voiceSource != null)
            voiceSource.PlayOneShot(koVoice);
    }

    /*sfx for block and whiff 
    public void PlayBlock()
    {
      if (block != null && sfxSource != null)
            sfxSource.PlayOneShot(block);
    }

    public void PlayWhiff()
    {
         if (whiff != null && sfxSource != null)
             sfxSource.PlayOneShot(whiff);
    }*/

    public void PlayPreRound()
    {
        if (preRoundVoice != null && voiceSource != null)
            voiceSource.PlayOneShot(preRoundVoice);
    }

    public void PlayRoundStart()
    {
        if (roundStartVoice != null && voiceSource != null)
            voiceSource.PlayOneShot(roundStartVoice);
    }
}