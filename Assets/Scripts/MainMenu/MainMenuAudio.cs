using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD.Studio;

public class MainMenuAudio : MonoBehaviour
{
    public static MainMenuAudio Instance;
    [SerializeField] private EventReference menuMusicEvent;
    [SerializeField] private EventReference buttonHoverEvent;
    [SerializeField] private EventReference buttonClickEvent;

    private EventInstance menuMusicInstance;


    private void Awake()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuMusicInstance = RuntimeManager.CreateInstance(menuMusicEvent);
        menuMusicInstance.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuMusicInstance.release();
    }

    public void PlayHoverSound()
    {
        RuntimeManager.PlayOneShot(buttonHoverEvent);
    }


    public void PlayClickSound()
    {
        RuntimeManager.PlayOneShot(buttonClickEvent);
    }
}
