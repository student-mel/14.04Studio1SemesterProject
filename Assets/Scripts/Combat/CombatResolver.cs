using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class CombatResolver : MonoBehaviour
{
    public static CombatResolver i;

    public Action OnBeat;

    InputBuffer inputBufferP1;
    InputBuffer inputBufferP2;

    int beatIndex = 0;

    List<AuthorisedAttack> authorisedAttacks = new List<AuthorisedAttack>();

    private void Awake()
    {
        if(i == null) { i = this; }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        authorisedAttacks.RemoveAll(a => a.hitApplied || Time.time > a.expiryTime);
    }

    private void OnEnable()
    {
        OnBeat += BeatUpdate;
    }
    private void OnDisable()
    {
        OnBeat -= BeatUpdate;
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
        
        CombatIntent p1 = inputBufferP1.GetIntentForBeat(beatIndex);
        CombatIntent p2 = inputBufferP2.GetIntentForBeat(beatIndex);

        CombatResult result = Resolve(p1, p2);

        if (result.clash)
        {
            //attacks clashes
        }
        else
        {
            NewAuthorisedAttack(result, beatIndex, Time.time + 0.5f);
        }
    }

    public CombatResult Resolve(CombatIntent p1, CombatIntent p2)
    {
        CombatResult result = new CombatResult();

        int p1Priority = GetPriority(p1.action);
        int p2Priority = GetPriority(p2.action);

        float p1Timing = GetTimingBonus(p1.timingOffset);
        float p2Timing = GetTimingBonus(p2.timingOffset);

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
            p1Wins ? p1Timing : p2Timing,
            ref result,
            p1Wins
        );

        return result;
    }

    void ApplyOutcome(
        CombatIntent _winner, 
        CombatIntent _loser, 
        float _winnerTiming,
        ref CombatResult _result, 
        bool _p1Wins)
    {
        bool heavyAttack = _winner.action == CombatActionType.HeavyAttack;

        int damage = heavyAttack ? 2 : 1;
        int hitstun = heavyAttack ? 2 : 1;

        if (_winnerTiming > 1.4f) hitstun++;

        if (_p1Wins)
        {
            _result.p2Hit = true;
            _result.p2Damage = damage;
            _result.p2Hitstun = hitstun;
        }
        else
        {
            _result.p1Hit = true;
            _result.p1Damage = damage;
            _result.p1Hitstun = hitstun;
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
            attack.damage = _result.p1Damage;
            attack.hitstun = _result.p1Hitstun;
        }
        else if (_result.p1Hit)
        {
            attack.id = 2;
            attack.damage = _result.p2Damage;
            attack.hitstun = _result.p2Hitstun;
        }

        authorisedAttacks.Add(attack);
    }

    int GetPriority(CombatActionType _action)
    {
        return (int)_action;
    }
    float GetTimingBonus(float _offset)
    {
        if (Mathf.Abs(_offset) < 0.03f) return 1.5f;
        if (Mathf.Abs(_offset) < 0.08f) return 1.0f;
        return 0.7f;
    }
}

public enum CombatActionType
{
    None = -1,
    Block = 0,
    LightAttack = 1,
    HeavyAttack = 2
}

public struct CombatIntent
{
    public int id;
    public CombatActionType action;
    public int beatIndex;
    public float timingOffset;
}

public struct CombatResult
{
    public bool p1Hit;
    public bool p2Hit;

    public int p1Damage;
    public int p2Damage;

    public bool clash;
    public bool parry;

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
