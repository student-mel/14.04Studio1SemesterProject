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

        float absBeat = Mathf.Abs(beatOffset);
        float absOffbeat = Mathf.Abs(offbeatOffset);

        string result;

        if (absOffbeat < absBeat && absOffbeat <= syncopatedWindow)
        {
            result = "Syncopated";
        }
        else if (absBeat <= perfectWindow)
        {
            result = "Perfect";
        }
        else if (absBeat <= greatWindow)
        {
            result = "Great";
        }
        else if (absBeat <= goodWindow)
        {
            result = "Good";
        }
        else
        {
            result = "Miss";
        }

        EventBus.Emit("actionResult", result);
    }
}