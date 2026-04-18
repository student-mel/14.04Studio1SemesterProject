using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource voiceSource;

    [Header("SFX")]
    public AudioClip[] hit;
    public AudioClip[] impact;
    public AudioClip ko;
    // public AudioClip block;   
    // public AudioClip whiff;   

    [Header("Announcer")]
    public AudioClip preRoundVoice;
    public AudioClip roundStartVoice;
    public AudioClip koVoice;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip fightMusic;

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
        if (sfxSource == null) return;

        bool playHitType = Random.value < 0.5f;

        if (playHitType && hit.Length > 0)
        {
            AudioClip clip = hit[Random.Range(0, hit.Length)];

            sfxSource.pitch = Random.Range(0.4f, 0.9f);
            sfxSource.PlayOneShot(clip);
        }
        else if (impact.Length > 0)
        {
            AudioClip clip = impact[Random.Range(0, impact.Length)];

            sfxSource.pitch = Random.Range(2f, 3f);
            sfxSource.PlayOneShot(clip);
        }

        // reset pitch 
        Invoke(nameof(ResetPitch), 0.5f);
    }
    private void ResetPitch()
    {
        if (sfxSource != null)
            sfxSource.pitch = 1f;
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

    public void PlayMusic()
    {
        if (musicSource != null && fightMusic != null)
        {
            musicSource.clip = fightMusic;
            musicSource.Play();
        }
    }

}