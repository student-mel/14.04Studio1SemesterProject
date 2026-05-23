using UnityEngine;

public class RhythmJudge : MonoBehaviour
{
    public float perfectWindow = 50f;
    public float greatWindow = 60f;
    //public float goodWindow = 100f;
    public float syncopatedWindow = 50f;

    private void OnEnable()
    {
        EventBus.Subscribe("start_action", OnAction);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("start_action", OnAction);
    }

    private void OnAction(object obj)
    {
        float beatOffset = RhythmStore.Instance.beatOffsetMs;
        float offbeatOffset = RhythmStore.Instance.offBeatOffsetMs;

        string result;

        if (offbeatOffset < beatOffset && offbeatOffset <= syncopatedWindow)
        {
            result = "Syncopated";
            EventBus.Emit("syncopated_action", obj);

        }
        else if (beatOffset <= perfectWindow)
        {
            result = "Perfect";
            EventBus.Emit("perfect_action", obj);

        }
        else if (beatOffset <= greatWindow)
        {
            result = "Great";
            EventBus.Emit("great_action", obj);

        }
        //else if (beatOffset <= goodWindow)
        //{
        //result = "Good";
        //}
        else
        {
            result = "Miss";
            EventBus.Emit("miss_action", obj);
        }

        EventBus.Emit("actionResult", new PlayerResult((int)obj, result));
        //RhythmStore.Instance.result = result;
        //Debug.Log($"Result = {result}");
    }
}

class PlayerResult
{
    public string Result;
    public int Index;
    public bool IsHit = false;
    
    public PlayerResult(int playerIndex, string playerResult, bool isHit = false)
    {
        Result = playerResult;
        Index = playerIndex;
        IsHit = isHit;
    }

    public override string ToString()
    {
        return Result;
    }
}