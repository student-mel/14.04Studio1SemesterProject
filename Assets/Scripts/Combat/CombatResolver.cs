using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class CombatResolver : MonoBehaviour
{
    public static CombatResolver i;

    public Action<CombatResult> OnCombatResolved;

    InputBuffer inputBuffer;

    CombatIntent p1Intent;
    CombatIntent p2Intent;

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
    // private void FixedUpdate()
    // {
    //     CombatResolve();
    // }
    //
    // private void CombatResolve()
    // {
    //     CombatIntent[] intents = inputBuffer.GetPendingIntents();
    //     CombatIntent p1 = intents.FirstOrDefault(i => i.player == 1);
    //     CombatIntent p2 = intents.FirstOrDefault(i => i.player == 2);
    //
    //     if (p1.attack.action == CombatActionType.None && p2.attack.action == CombatActionType.None) return;
    //
    //     CombatResult result = new CombatResult();
    //
    //     result = Resolve(p1, p2);
    //
    //     DebugResult(result);
    //
    //     if (result.clash)
    //     {
    //         //attacks clashes
    //     }
    //     else
    //     {
    //         NewAuthorisedAttack(result);
    //     }
    //
    //     OnCombatResolved?.Invoke(result);
    // }

    public void SetInputBuffer(InputBuffer _input)
    {
        inputBuffer = _input;
    }

    public CombatResult Resolve(CombatIntent p1, CombatIntent p2)
    {
        CombatResult result = new CombatResult();

        if (p1.attack.action == CombatActionType.None && p2.attack.action == CombatActionType.None)
        {
            result.idle = true;
            return result;
        }

        int p1Priority = GetPriority(p1.attack.action);
        int p2Priority = GetPriority(p2.attack.action);

        float p1Score = p1Priority;
        float p2Score = p2Priority;

        result.startFrame = p1.hitFrame < p2.hitFrame ? p1.hitFrame : p2.hitFrame;
        result.endFrame = p1.endFrame > p2.endFrame ? p1.endFrame : p2.endFrame;

        if(Mathf.Approximately(p1Score, p2Score))
        {
            result.clash = true;
            return result;
        }

        bool p1Wins = p1Score > p2Score;

        ApplyOutcome(
            _winner: p1Wins ? p1 : p2,
            _loser: p1Wins ? p2 : p1,
            ref result, 
            p1Wins
        );

        return result;
    }

    void ApplyOutcome(
        CombatIntent _winner,
        CombatIntent _loser,
        ref CombatResult _result, 
        bool _p1Wins)
    {
        bool heavyAttack = _winner.attack.action == CombatActionType.HeavyAttack;

        int damage = 1;
        if (heavyAttack) damage++;

        int hitstun = 1;
        if (heavyAttack) hitstun++;


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

    void NewAuthorisedAttack(CombatResult _result)
    {
        if (!_result.p1Hit && !_result.p2Hit) return;

        AuthorisedAttack attack = new AuthorisedAttack();

        attack.expiryTime = _result.endFrame;
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

    void DebugResult(CombatResult _result)
    {
        if (!debug) return;

        string debugResult = "";
        //debugResult += $"Frame {GameClock.Frame}\n";
        //debugResult += $"Player 1 Intent: {p1.action} ; Player 2 Intent: {p2.action}\n";
        if (_result.idle)
        {
            debugResult += $"Players idling: {_result.idle}\n";
        }
        //else if (result.clash)
        //{
        //    debugResult += $"Attacks clash: {result.clash}\n";
        //}
        if (_result.p1Hit)
        {
            debugResult += $"Player 1 should be hit\n";
            debugResult += $"Player 1 should take {_result.p1Damage} damage\n";
            debugResult += $"Player 1 should be stunned for {_result.p1Hitstun} beat(s)\n";
        }
        else if (_result.p2Hit)
        {
            debugResult += $"Player 2 should be hit\n";
            debugResult += $"Player 2 should take {_result.p2Damage} damage\n";
            debugResult += $"Player 2 should be stunned for {_result.p2Hitstun} beat(s)\n";
        }
        if (debugResult != "")
            Debug.Log(debugResult);
    }
}

public enum CombatActionType
{
    None = -1,
    Block = 0,
    LightAttack = 1,
    HeavyAttack = 2
}

public struct AttackData
{
    public InputType[] inputs;
    public CombatActionType action;
    public int frame;
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    public int chainStartFrame;
    public int chainEndFrame;
}

public struct CombatIntent
{
    public int player;
    public AttackData attack;
    public int hitFrame;
    public int endFrame;
}

public struct CombatResult
{
    public float startFrame;
    public float endFrame;

    public bool p1Hit;
    public bool p2Hit;

    public bool idle;
    public bool clash;

    public int p1Damage;
    public int p2Damage;

    public int p1Hitstun;
    public int p2Hitstun;
}
public struct AuthorisedAttack
{
    public int id;
    public int damage;
    public int hitstun;
    public float expiryTime;
    public bool hitApplied;
}
