using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonAudio : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static float lastClickTime;
    [SerializeField] private float hoverBlockTime = 0.1f;

    public void OnSelect(BaseEventData eventData)
    {
        if (Time.unscaledTime - lastClickTime < hoverBlockTime) return;
        MainMenuAudio.Instance?.PlayHoverSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.unscaledTime - lastClickTime < hoverBlockTime) return;
        MainMenuAudio.Instance?.PlayHoverSound();
    }

    public void PlayClick()
    {
        lastClickTime = Time.unscaledTime;
        MainMenuAudio.Instance?.PlayClickSound();
    }
}
