using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    List<BufferedInput> P1Inputs_Direction = new List<BufferedInput>();
    List<BufferedInput> P2Inputs_Direction = new List<BufferedInput>();

    List<BufferedInput> P1Inputs_Attack = new List<BufferedInput>();
    List<BufferedInput> P2Inputs_Attack = new List<BufferedInput>();

    AttackData p1Attack;
    AttackData p2Attack;

    public CombatIntent P1Intent;
    public CombatIntent P2Intent;

    const int INPUT_BUFFER_FRAME = 6;
    const int SIMULTANEOUS_FRAMES = 2;
    const int VALID_INPUTS_ARRAY_SIZE = 6;

    #region Input Handling
    public void ClearExpiredInputs(float time)
    {
        P1Inputs_Direction.RemoveAll(i => i.frame < GameClock.Frame - INPUT_BUFFER_FRAME);
        P2Inputs_Direction.RemoveAll(i => i.frame < GameClock.Frame - INPUT_BUFFER_FRAME);

        P1Inputs_Attack.RemoveAll(i => i.frame < GameClock.Frame - INPUT_BUFFER_FRAME);
        P2Inputs_Attack.RemoveAll(i => i.frame < GameClock.Frame - INPUT_BUFFER_FRAME);

        if (IsExpired(1)) { p1Attack = new AttackData(); }
        if (IsExpired(2)) { p2Attack = new AttackData(); }
    }

    public void AddInput(int _playerIndex, InputType _input)
    {
        if(_playerIndex == 1)
            P1Inputs_Direction.Add(new BufferedInput
            {
                input = _input,
                frame = GameClock.Frame
            });
        else if(_playerIndex == 2)
            P2Inputs_Direction.Add(new BufferedInput
            {
                input = _input,
                frame = GameClock.Frame
            });
    }

    List<BufferedInput> tempAttackInputs = new List<BufferedInput>();
    List<BufferedInput> tempDirectionInputs = new List<BufferedInput>();

    List<BufferedInput> tempAttackList = new List<BufferedInput>();
    public void AddAttackInput(int _playerIndex, InputType _input)
    {

        if (_playerIndex == 1)
        {
            tempAttackList = P1Inputs_Attack;
        }
        else if(_playerIndex == 2)
        {
            tempAttackList = P2Inputs_Attack;
            tempDirectionInputs = new List<BufferedInput>(P2Inputs_Direction);
        }

        tempAttackList.Add(new BufferedInput
        {
            input = _input,
            frame = GameClock.Frame
        });
    }
    private bool IsSimultaneous(BufferedInput _a, BufferedInput _b)
    {
        return Mathf.Abs(_a.frame - _b.frame) <= SIMULTANEOUS_FRAMES;
    }
    private void AddValidMoves(int _playerIndex, List<BufferedInput> _dir, List<BufferedInput> _att)
    {
        InputType[] currMove = new InputType[VALID_INPUTS_ARRAY_SIZE];
        AttackData attack = new AttackData();

        for (int i = 0; i < 3; i++)
        {
            if (_dir.Count <= i) break;
            if (_dir[i].input != InputType.None)
            {
                currMove[i] = _dir[i].input;
            }
        }

        for (int i = 3; i < 6; i++)
        {
            if (_att.Count <= i - 3) break;
            if (_att[i - 3].input != InputType.None)
            {
                currMove[i] = _att[i - 3].input;
            }
        }

        attack.inputs = currMove;
        attack = GetAttackForInput(ref attack);
        Debug.Log(currMove[3]);

        if (_playerIndex == 1)
        {
            p1Attack = attack;
        }
        else if( _playerIndex == 2)
        {
            p2Attack = attack;
        }
    }
    public bool CanAcceptNextInput(int _playerIndex)
    {
        if (_playerIndex == 1)
            return p1Attack.inputs != null && p1Attack.frame >= p1Attack.chainStartFrame && p1Attack.frame <= p1Attack.chainEndFrame;
        else if (_playerIndex == 2)
            return p2Attack.inputs != null && p2Attack.frame >= p2Attack.chainStartFrame && p2Attack.frame <= p2Attack.chainEndFrame;
        else return false;
    }
    public bool IsExpired(int _playerIndex)
    {
        if (_playerIndex == 1)
            return p1Attack.inputs != null && p1Attack.frame > p1Attack.chainEndFrame;
        else if (_playerIndex == 2)
            return p2Attack.inputs != null && p2Attack.frame > p2Attack.chainEndFrame;
        else return false;
    }
    #endregion

    private void Update()
    {
        if (P1Inputs_Attack.Count > 0)
        {
            tempAttackInputs.Clear();

            if (P1Inputs_Attack[0].frame < GameClock.Frame - SIMULTANEOUS_FRAMES)
            {
                BufferedInput a = P1Inputs_Attack[0];
                tempAttackInputs.Add(a);

                if (P1Inputs_Attack.Count > 1)
                {
                    BufferedInput b = P1Inputs_Attack[1];

                    if (IsSimultaneous(a, b))
                    {
                        tempAttackInputs.Add(b);
                    }
                }

                AddValidMoves(1, P1Inputs_Direction, tempAttackInputs);
            }
        }
        if (P2Inputs_Attack.Count > 0)
        {
            tempAttackInputs.Clear();

            if (P2Inputs_Attack[0].frame < GameClock.Frame - SIMULTANEOUS_FRAMES)
            {
                BufferedInput a = P2Inputs_Attack[0];
                tempAttackInputs.Add(a);

                if (P2Inputs_Attack.Count > 1)
                {
                    BufferedInput b = P2Inputs_Attack[1];

                    if (IsSimultaneous(a, b))
                    {
                        tempAttackInputs.Add(b);
                    }
                }

                AddValidMoves(2, P2Inputs_Direction, tempAttackInputs);
            }
        }
    }

    private void Start()
    {
        CombatResolver.i.SetInputBuffer(this);
    }

    public CombatIntent[] GetPendingIntents()
    {
        CombatIntent p1Intent = new CombatIntent();
        CombatIntent p2Intent = new CombatIntent();

        p1Intent.player = 1;
        p2Intent.player = 2;

        p1Intent.attack = p1Attack;
        p2Intent.attack = p2Attack;

        p1Intent.hitFrame = p1Attack.frame + p1Attack.startupFrames;
        p1Intent.endFrame = p1Intent.hitFrame + p1Attack.activeFrames;

        return new CombatIntent[] { p1Intent, p2Intent };
    }
    AttackData GetAttackForInput(ref AttackData _attack)
    {
        _attack.action = GetActionForAttack(_attack);
        _attack.frame = GameClock.Frame;
        _attack.startupFrames = 5;
        _attack.activeFrames = 10;
        _attack.recoveryFrames = 5;
        _attack.chainStartFrame = 12;
        _attack.chainEndFrame = 20;
        return _attack;
    }
    CombatActionType GetActionForAttack(AttackData _attack)
    {
        if (_attack.inputs[3] == InputType.Light)
            return CombatActionType.LightAttack;
        else return CombatActionType.None;
    }
}

public struct BufferedInput
{
    public InputType input;
    public int frame;
}
public enum InputType
{
    None,
    Left,
    LeftUp,
    LeftDown,
    Right,
    RightUp,
    RightDown,
    Up,
    Down,
    Light,
    Medium,
    Heavy
}
