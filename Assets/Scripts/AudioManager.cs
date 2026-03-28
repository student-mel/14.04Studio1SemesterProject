using UnityEngine; //

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource voiceSource;

    [Header("SFX")]
    public AudioClip hit;
    public AudioClip ko;

    [Header("Announcer")]
    public AudioClip roundStartVoice;
    public AudioClip koVoice;

    void Awake()
    {
        Instance = this;
    }

    public void PlayHit() => sfxSource.PlayOneShot(hit);

    public void PlayKO()
    {
        sfxSource.PlayOneShot(ko);
        voiceSource.PlayOneShot(koVoice);
    }

    public void PlayRoundStart()
    {
        voiceSource.PlayOneShot(roundStartVoice);
    }
}
