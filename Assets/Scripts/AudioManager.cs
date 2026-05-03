using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private bool walkSoundPlaying = false;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource voiceSource;

    [Header("SFX")]
    //public AudioClip[] hit;
    //public AudioClip[] impact;
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

    [Header("FMOD")]
    [SerializeField] private EventReference hitEvent;
    [SerializeField] private EventReference jumpEvent;
    [SerializeField] private EventReference lightAttackEvent;
    [SerializeField] private EventReference mediumAttackEvent;
    [SerializeField] private EventReference heavyAttackEvent;
    [SerializeField] private EventReference walkEvent;
    private FMOD.Studio.EventInstance walkInstance;

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

    public void PlayHit(GameObject hitTarget)
    {
        RuntimeManager.PlayOneShot(hitEvent);
    }

    public void PlayJump(GameObject jumper)
    {
        RuntimeManager.PlayOneShot(jumpEvent);
    }

    public void PlayLightAttack(GameObject target)
    {
        RuntimeManager.PlayOneShot(lightAttackEvent);
    }

    public void PlayMediumAttack(GameObject target)
    {
        RuntimeManager.PlayOneShot(mediumAttackEvent);
    }

    public void PlayHeavyAttack(GameObject target)
    {
        RuntimeManager.PlayOneShot(heavyAttackEvent);
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

    public void PlayWalk()
    {
        if (walkSoundPlaying) return;

        walkInstance = RuntimeManager.CreateInstance(walkEvent);
        walkInstance.start();

        walkSoundPlaying = true;
    }

    public void StopWalk()
    {
        if (!walkSoundPlaying) return;

        walkInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        walkInstance.release();

        walkSoundPlaying = false;
    }

    //public void PlayMusic()
    //{
    //    if (musicSource != null && fightMusic != null)
    //    {
    //        musicSource.clip = fightMusic;
    //        musicSource.Play();
    //    }
    //}

}