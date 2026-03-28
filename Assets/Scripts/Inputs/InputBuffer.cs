using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    List<BufferedInput> P1Inputs_Direction = new List<BufferedInput>();
    List<BufferedInput> P2Inputs_Direction = new List<BufferedInput>();

    List<BufferedInput> P1Inputs_Attack = new List<BufferedInput>();
    List<BufferedInput> P2Inputs_Attack = new List<BufferedInput>();

    InputData p1CurrentMove;
    InputData p2CurrentMove;

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

        if (IsExpired(1)) { p1CurrentMove = new InputData(); p1Attack = new AttackData(); }
        if (IsExpired(2)) { p2CurrentMove = new InputData(); p2Attack = new AttackData(); }
    }

    public void P1AddInput(InputType _input)
    {
        P1Inputs_Direction.Add(new BufferedInput
        {
            input = _input,
            frame = GameClock.Frame
        });
    }
    public void P2AddInput(InputType _input)
    {
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
        tempAttackInputs.Clear();
        tempDirectionInputs.Clear();

        if (_playerIndex == 1)
        {
            tempAttackList = P1Inputs_Attack;
            tempDirectionInputs = new List<BufferedInput>(P1Inputs_Direction);
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

        if(tempAttackList.Count > 0)
        {
            if (tempAttackList[0].frame < GameClock.Frame - SIMULTANEOUS_FRAMES)
            {
                BufferedInput a = tempAttackList[0];
                tempAttackInputs.Add(a);

                if (tempAttackList.Count > 1)
                {
                    BufferedInput b = tempAttackList[1];

                    if (IsSimultaneous(a, b))
                    {
                        tempAttackInputs.Add(b);
                    }
                }

                AddValidMoves(_playerIndex, tempDirectionInputs, tempAttackInputs);
            }
        }
    }
    private bool IsSimultaneous(BufferedInput _a, BufferedInput _b)
    {
        return Mathf.Abs(_a.frame - _b.frame) <= SIMULTANEOUS_FRAMES;
    }
    private void AddValidMoves(int _playerIndex, List<BufferedInput> _dir, List<BufferedInput> _att)
    {
        InputType[] currMove = new InputType[VALID_INPUTS_ARRAY_SIZE];
        InputData data = new InputData();
        AttackData attack = new AttackData();

        for (int i = 0; i < 3; i++)
        {
            if (_dir[i].input != InputType.None)
            {
                currMove[i] = _dir[i].input;
            }
        }

        for (int i = 3; i < 6; i++)
        {
            if (_att[i].input != InputType.None)
            {
                currMove[i] = _dir[i].input;
            }
        }

        data.inputs = currMove;
        data.frame = GameClock.Frame;

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
            return p1CurrentMove.inputs != null && p1CurrentMove.frame >= p1Attack.chainStartFrame && p1CurrentMove.frame <= p1Attack.chainEndFrame;
        else if (_playerIndex == 2)
            return p2CurrentMove.inputs != null && p2CurrentMove.frame >= p2Attack.chainStartFrame && p2CurrentMove.frame <= p2Attack.chainEndFrame;
        else return false;
    }
    public bool IsExpired(int _playerIndex)
    {
        if (_playerIndex == 1)
            return p1CurrentMove.inputs != null && p1CurrentMove.frame > p1Attack.chainEndFrame;
        else if (_playerIndex == 2)
            return p2CurrentMove.inputs != null && p2CurrentMove.frame > p2Attack.chainEndFrame;
        else return false;
    }
    #endregion

    private void Start()
    {
        CombatResolver.i.SetInputBuffer(this);
    }

    public CombatIntent[] GetPendingIntents()
    {
        CombatIntent p1Intent = new CombatIntent();
        CombatIntent p2Intent = new CombatIntent();

        AttackData p1Attack;
        AttackData p2Attack;

        p1Attack = GetAttackForInput(p1CurrentMove);
        p2Attack = GetAttackForInput(p2CurrentMove);

        p1Intent.player = 1;
        p2Intent.player = 2;

        p1Intent.attack = p1Attack;
        p2Intent.attack = p2Attack;

        p1Intent.hitFrame = p1Attack.frame + p1Attack.startupFrames;
        p1Intent.endFrame = p1Intent.hitFrame + p1Attack.activeFrames;

        return new CombatIntent[] { p1Intent, p2Intent };
    }
    AttackData GetAttackForInput(InputData _input)
    {
        AttackData _attack = new AttackData();
        _attack.action = GetActionForInput(_input);
        _attack.frame = _input.frame;
        _attack.startupFrames = 5;
        _attack.activeFrames = 10;
        _attack.recoveryFrames = 5;
        _attack.chainStartFrame = 12;
        _attack.chainEndFrame = 20;
        return _attack;
    }
    CombatActionType GetActionForInput(InputData _input)
    {
        if (_input.inputs != null)
            return CombatActionType.LightAttack;
        else return CombatActionType.None;
    }
}

public struct BufferedInput
{
    public InputType input;
    public int frame;
}
public struct InputData
{
    public InputType[] inputs;
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
