using UnityEngine;

public class RhythmStore : MonoBehaviour
{
    public static RhythmStore Instance;

    [Header("Music")]
    public BGM_Rhythm bgm;

    public float beatDuration;
    public float musicTimeMs;
    public bool isPlaying;

    [Header("Metronome")] 
    public int activeBeat;
    public int currentBeatIndex;

    /*[Header("Judge")]
    public float lastTimingOffsetMs;
    public string lastJudgement;*/

    /*[Header("Combat")]
    public float lastDamage;*/

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
