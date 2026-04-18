using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TMP_Text timerText;

    public int roundTime = 60;
    private float currentTime;

    private bool isRunning = false;

    void Start()
    {
        currentTime = roundTime;
        UpdateText();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            currentTime = 0;

        UpdateText();
    }

    public void StartTimer()
    {
        currentTime = roundTime;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    void UpdateText()
    {
        timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
