using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FmodBGM : MonoBehaviour
{
    [SerializeField] private EventReference musicEvent;
    private EventInstance musicInstance;
    private bool isPlaying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        EventBus.Subscribe("start_rhythm", PlayMusic);
        EventBus.Subscribe("end_rhythm", StopMusic);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("start_rhythm", PlayMusic);
        EventBus.Unsubscribe("end_rhythm", StopMusic);
    }

    private void PlayMusic(object obj)
    {
        if (isPlaying) return;

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
        isPlaying = true;
    }

    private void StopMusic(object obj)
    {
        if (!isPlaying) return;

        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
        isPlaying = false;
    }

    private void OnDestroy()
    {
        if (isPlaying)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
    }
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
