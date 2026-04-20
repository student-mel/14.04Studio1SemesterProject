using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnEnable()
    {
        EventBus.Subscribe("start_rhythm", PlaySong);
        EventBus.Subscribe("end_rhythm", StopSong);
    }

    private void StopSong(object obj)
    {
        audioSource.Stop();
        RhythmStore.Instance.isPlaying = false;
        EventBus.Emit("song_stopped");
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("start_rhythm", PlaySong);
        EventBus.Unsubscribe("end_rhythm", StopSong);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = RhythmStore.Instance.bgm.clip;
    }

    public void PlaySong(object obj)
    {
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
