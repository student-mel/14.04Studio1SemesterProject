using UnityEngine;

public class RhythmJudge : MonoBehaviour
{
    public float perfectWindow = 30f;
    public float greatWindow = 60f;
    public float goodWindow = 100f;
    public float syncopatedWindow = 80f;

    private void OnEnable()
    {
        EventBus.Subscribe("action", OnAction);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("action", OnAction);
    }

    private void OnAction(object obj)
    {
        float beatOffset = RhythmStore.Instance.beatOffsetMs;
        float offbeatOffset = RhythmStore.Instance.offBeatOffsetMs;

        string result;

        if (offbeatOffset < beatOffset && offbeatOffset <= syncopatedWindow)
        {
            result = "Syncopated";
        }
        else if (beatOffset <= perfectWindow)
        {
            result = "Perfect";
        }
        else if (beatOffset <= greatWindow)
        {
            result = "Great";
        }
        else if (beatOffset <= goodWindow)
        {
            result = "Good";
        }
        else
        {
            result = "Miss";
        }

        EventBus.Emit("actionResult", result);
        Debug.Log(result);
    }
}