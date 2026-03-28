using UnityEngine;
using TMPro;
public class timerUI : MonoBehaviour
{
    public TMP_Text timerText;
    public int RoundTime;
    private int CurrentTime;

    private void CountDown()
    {
        CurrentTime -= 1;
        UpdateText();
    }


    private void UpdateText()
    {
        timerText.text = CurrentTime.ToString();
    }

}

