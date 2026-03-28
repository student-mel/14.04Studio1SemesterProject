using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Collections;

public class CombatResolver : MonoBehaviour
{
    public static CombatResolver i;

    public Action OnBeat;

    InputBuffer inputBufferP1;
    InputBuffer inputBufferP2;

    int beatIndex = 0;

    public bool debug;

    public List<AuthorisedAttack> authorisedAttacks { get; private set; } = new List<AuthorisedAttack>();

    private void Awake()
    {
        if(i == null) { i = this; }
        else if(i != this) { Destroy(gameObject); }
    }

    private void Update()
    {
        authorisedAttacks.RemoveAll(a => a.hitApplied || Time.time > a.expiryTime);
    }

    private void OnEnable()
    {
        if (TestBeatClock.Clock == null)
            TestBeatClock.CreateClock();
        TestBeatClock.Clock.CustomUpdate += BeatUpdate;
        
        //OnBeat += BeatUpdate;
    }
    private void OnDisable()
    {
        TestBeatClock.Clock.CustomUpdate -= BeatUpdate;

        //OnBeat -= BeatUpdate;
    }

    public void SetInputBuffer(int _index, InputBuffer _input)
    {
        if(_index == 1)
            inputBufferP1 = _input;
        else if( _index == 2)
            inputBufferP2 = _input;
    }

    public void BeatUpdate()
    {
        beatIndex++;

        StartCoroutine(ScheduleCombatResolve(Time.time));
    }

    IEnumerator ScheduleCombatResolve(float _time)
    {
        float resolveTime = _time + TestBeatClock.Interval * 0.5f;

        yield return new WaitWhile(() => resolveTime > Time.time);

        CombatIntent p1 = inputBufferP1.GetIntentForBeat(_time);
        CombatIntent p2 = inputBufferP2.GetIntentForBeat(_time);

        CombatResult result = Resolve(p1, p2);

        string debugResult = "";
        debugResult += $"Beat{beatIndex}\n";
        debugResult += $"Player 1 Intent: {p1.action} ; Player 2 Intent: {p2.action}\n";
        if (result.idle)
        {
            debugResult += $"Players idling: {result.idle}\n";
        }
        else if (result.clash)
        {
            debugResult += $"Attacks clash: {result.clash}\n";
        }
        if (result.p1Hit)
        {
            debugResult += $"\nPlayer 2 hits {result.p2Beat.ToString()} beat\n";
            debugResult += $"Player 1 should be hit\n";
            debugResult += $"Player 1 should take {result.p1Damage} damage\n";
            debugResult += $"Player 1 should be stunned for {result.p1Hitstun} beat(s)\n";
        }
        else if (result.p2Hit)
        {
            debugResult += $"\nPlayer 1 hits {result.p1Beat.ToString()} beat\n";
            debugResult += $"Player 2 should be hit\n";
            debugResult += $"Player 2 should take {result.p2Damage} damage\n";
            debugResult += $"Player 2 should be stunned for {result.p2Hitstun} beat(s)\n";
        }
        if (debugResult != "" && debug)
            Debug.Log(debugResult);

        if (result.clash)
        {
            //attacks clashes
        }
        else
        {
            NewAuthorisedAttack(result, beatIndex, Time.time + TestBeatClock.Interval);
        }
    }

    public CombatResult Resolve(CombatIntent p1, CombatIntent p2)
    {
        CombatResult result = new CombatResult();

        if(p1.action == CombatActionType.None && p2.action == CombatActionType.None)
        {
            result.idle = true;
            return result;
        }

        int p1Priority = GetPriority(p1.action);
        int p2Priority = GetPriority(p2.action);

        float p1Timing = GetTimingBonus(p1.timingOffset);
        float p2Timing = GetTimingBonus(p2.timingOffset);

        BeatJudgement p1Beat = GetTiming(p1.timingOffset);
        BeatJudgement p2Beat = GetTiming(p2.timingOffset);

        float p1Score = p1Priority * p1Timing;
        float p2Score = p2Priority * p2Timing;

        if(Mathf.Approximately(p1Score, p2Score))
        {
            result.clash = true;
            return result;
        }

        bool p1Wins = p1Score > p2Score;

        ApplyOutcome(
            _winner: p1Wins ? p1 : p2,
            _loser: p1Wins ? p2 : p1,
            p1Wins ? p1Beat : p2Beat,
            p1Wins ? p2Beat : p1Beat,
            ref result,
            p1Wins
        );

        return result;
    }

    void ApplyOutcome(
        CombatIntent _winner, 
        CombatIntent _loser, 
        BeatJudgement _winnerBeat,
        BeatJudgement _loserBeat,
        ref CombatResult _result, 
        bool _p1Wins)
    {
        bool heavyAttack = _winner.action == CombatActionType.HeavyAttack;

        int damage = 1;
        if (heavyAttack) damage++;
        if(_winnerBeat == BeatJudgement.Perfect) damage++;

        int hitstun = 1;
        if (heavyAttack) hitstun++;
        if (_winnerBeat == BeatJudgement.Perfect) hitstun++;
        else if (_winnerBeat == BeatJudgement.Miss) hitstun = 0;


        if (_p1Wins)
        {
            _result.p2Hit = true;
            _result.p2Damage = damage;
            _result.p2Hitstun = hitstun;
            _result.p2Beat = _loserBeat;
            _result.p1Beat = _winnerBeat;
        }
        else
        {
            _result.p1Hit = true;
            _result.p1Damage = damage;
            _result.p1Hitstun = hitstun;
            _result.p1Beat = _loserBeat;
            _result.p2Beat = _winnerBeat;
        }
    }

    void NewAuthorisedAttack(CombatResult _result, int beat, float _expiry)
    {
        if (!_result.p1Hit && !_result.p2Hit) return;

        AuthorisedAttack attack = new AuthorisedAttack();

        attack.resolvedBeat = beat;
        attack.expiryTime = _expiry;
        attack.hitApplied = false;

        if (_result.p2Hit)
        {
            attack.id = 1;
            attack.damage = _result.p2Damage;
            attack.hitstun = _result.p2Hitstun;
        }
        else if (_result.p1Hit)
        {
            attack.id = 2;
            attack.damage = _result.p1Damage;
            attack.hitstun = _result.p1Hitstun;
        }

        authorisedAttacks.Add(attack);
    }

    int GetPriority(CombatActionType _action)
    {
        return (int)_action;
    }
    float GetTimingBonus(float _offset)
    {
        if (Mathf.Abs(_offset) < TestBeatClock.Interval * 0.066f) return 1.5f;
        if (Mathf.Abs(_offset) < TestBeatClock.Interval * 0.22f) return 1.0f;
        return 0.7f;
    }
    BeatJudgement GetTiming(float _offset)
    {
        if (Mathf.Abs(_offset) < TestBeatClock.Interval * 0.066f) return BeatJudgement.Perfect;
        if (Mathf.Abs(_offset) < TestBeatClock.Interval * 0.22f) return BeatJudgement.Good;
        return BeatJudgement.Miss;
    }
}

public enum CombatActionType
{
    None = -1,
    Block = 0,
    LightAttack = 1,
    HeavyAttack = 2
}

public enum BeatJudgement
{
    None,
    Miss,
    Good,
    Perfect
}

public struct CombatIntent
{
    public int id;
    public CombatActionType action;
    public float beatTime;
    public float timingOffset;
}

public struct CombatResult
{
    public bool p1Hit;
    public bool p2Hit;

    public BeatJudgement p1Beat;
    public BeatJudgement p2Beat;

    public int p1Damage;
    public int p2Damage;

    public bool idle;
    public bool clash;

    public int p1Hitstun;
    public int p2Hitstun;
}
public struct AuthorisedAttack
{
    public int id;
    public int resolvedBeat;
    public int damage;
    public int hitstun;
    public float expiryTime;
    public bool hitApplied;
}
