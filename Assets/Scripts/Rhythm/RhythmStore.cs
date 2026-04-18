using UnityEngine;

public class RhythmStore : MonoBehaviour
{
    public static RhythmStore Instance;

    [Header("Music")]
    //public SO_Bgm bgm;
    public RockBGM bgm;

    public float beatDuration;
    public float musicTimeMs;
    public bool isPlaying;

    [Header("Metronome")] 
    public int activeBeat;
    public int currentBeatIndex;

    [Header("Judge")]
    public float beatOffsetMs;
    public float offBeatOffsetMs;
    public string result;

    [Header("Combat")]
    public bool actionQueued;
    public bool actionActive;
    public string currentMoveName;
    public int currentMoveStartupBeats;
    public int currentMoveActiveBeats;
    public int currentMoveRecoveryBeats;
    public int currentMovePhaseBeat;
    public string currentMovePhase; 
    public string lastActionJudgement;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMusicTime(float timeMs)
    {
        musicTimeMs = timeMs;
    }

    public void SetBeat(int beatIndex, float beatTime)
    {
        currentBeatIndex = beatIndex;
    }

    /*public void SetJudgement(string judgement, float offset)
    {
        lastJudgement = judgement;
        lastTimingOffsetMs = offset;
    }*/

    /*public void SetDamage(float dmg)
    {
        lastDamage = dmg;
    }*/
}
