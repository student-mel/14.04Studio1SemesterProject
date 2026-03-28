using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = RhythmStore.Instance.bgm.clip;
        audioSource.Play();
        RhythmStore.Instance.isPlaying = true;
        EventBus.Emit("song_started");
    }

    private void Update()
    {
        if (!audioSource.isPlaying) return;

        float timeMs = audioSource.time * 1000f;

        RhythmStore.Instance.SetMusicTime(timeMs);
        EventBus.Emit("music_time", timeMs);
    }
}
