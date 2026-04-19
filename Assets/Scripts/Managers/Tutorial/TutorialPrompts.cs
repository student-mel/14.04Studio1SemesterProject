using UnityEngine;
using TMPro;

public class SimpleTutorialPrompts : MonoBehaviour
{
    public TMP_Text promptText;

    private int step = 0;

    void Start()
    {
        ShowStep();
    }

    public void NextStep()
    {
        step++;
        ShowStep();
    }

    void ShowStep()
    {
        switch (step)
        {
            case 0:
                promptText.text = "Move Left and Right";
                break;
            case 1:
                promptText.text = "Attack on the Beat";
                break;
            case 2:
                promptText.text = "Try chaining attacks";
                break;
            default:
                promptText.text = "You're ready. Fight!";
                break;
        }
    }
}
