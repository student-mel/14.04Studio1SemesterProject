using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    List<BufferedInput> bufferedInputs = new List<BufferedInput>();

    public PlayerInputHandler handler;
    
    List<BufferedInput> P1Inputs_Direction = new List<BufferedInput>();
    List<BufferedInput> P2Inputs_Direction = new List<BufferedInput>();

    List<BufferedInput> P1Inputs_Attack = new List<BufferedInput>();
    List<BufferedInput> P2Inputs_Attack = new List<BufferedInput>();

    AttackData p1Attack;
    AttackData p2Attack;

    const int INPUT_BUFFER_MS_TIME = 200;
    const int SIMULTANEOUS_FRAMES = 2;
    const int VALID_INPUTS_ARRAY_SIZE = 6;

    public bool hasInputThisFrame = false;
    public bool movementStoppedThisFrame = false;
    private BufferedInput _bufferedInput1;

    //#region Input Handling
    public void ClearExpiredInputs()
    {
        bufferedInputs.RemoveAll(i => i.time < RhythmStore.Instance.musicTimeMs - INPUT_BUFFER_MS_TIME);
    
        //if (IsExpired(1)) { p1Attack = new AttackData(); }
        //if (IsExpired(2)) { p2Attack = new AttackData(); }
    }

    public void ClearAllInputs()
    {
        bufferedInputs.Clear();
    }
    
    const float WHOLE = 360f;
    const float SIXTEENTH = WHOLE / 16f;
    const float EIGHTH = WHOLE / 8f;
    const float QUARTER = WHOLE / 4f;
    const float HALF = WHOLE / 2f;

    public void AddInputStart(Vector2 _input)
    {
        if (Mathf.Abs(_input.x) < 0.01f && Mathf.Abs(_input.y) < 0.01f) return;
        
        InputType dirInput = GetDirectionalInputType(_input);
        
        if (bufferedInputs.Count > 0)
            SetBufferedInputsTime(bufferedInputs, RhythmStore.Instance.musicTimeMs);
        
        bufferedInputs.Add(new BufferedInput
        {
            input = dirInput,
            type = MoveType.Movement,
            frame = FrameClock.Frame,
            time = RhythmStore.Instance.musicTimeMs,
        });
        hasInputThisFrame = true;
        OnDirectionalInput(dirInput);
    }
    public void AddInputStart(InputType _input)
    {
        if (bufferedInputs.Count > 0)
            SetBufferedInputsTime(bufferedInputs, RhythmStore.Instance.musicTimeMs);
        
        bufferedInputs.Add(new BufferedInput
        {
            input = _input,
            type = MoveType.Attack,
            frame = FrameClock.Frame,
            time = RhythmStore.Instance.musicTimeMs,
        });
        hasInputThisFrame = true;

        OnAttackInput(_input);
    }
    
    public void AddInput(Vector2 _input)
    {
        if (Mathf.Abs(_input.x) < 0.01f && Mathf.Abs(_input.y) < 0.01f) return;
        
        InputType dirInput = GetDirectionalInputType(_input);

        hasInputThisFrame = true;
        
        if (bufferedInputs.Count > 0)
        {
            for (int i = bufferedInputs.Count - 1; i >= 0; i--)
            {
                BufferedInput input = bufferedInputs[i];
                if(input.type == MoveType.Attack) continue;
                else if (input.type == MoveType.Movement)
                {
                    if (input.input == dirInput)
                    {
                        input.frame = FrameClock.Frame;
                        input.time = RhythmStore.Instance.musicTimeMs;
                        goto OnInputAdded;
                    }
                    else
                    {
                        goto AddBufferedInput;
                    }
                }
            }
        }
        AddBufferedInput:
        bufferedInputs.Add(new BufferedInput
        {
            input = dirInput,
            type = MoveType.Movement,
            frame = FrameClock.Frame,
            time = RhythmStore.Instance.musicTimeMs,
        });
        
        SetBufferedInputsTime(bufferedInputs, RhythmStore.Instance.musicTimeMs);
        
        OnInputAdded:
        OnDirectionalInput(dirInput);
    }
    public void AddInput(InputType _input)
    {
        hasInputThisFrame = true;
        
        if (bufferedInputs.Count > 0)
        {
            BufferedInput lastInput = bufferedInputs[^1];
            if (lastInput.input == _input)
            {
                lastInput.frame = FrameClock.Frame;
                lastInput.time = RhythmStore.Instance.musicTimeMs;
                goto OnInputAdded;
            }
        }
        bufferedInputs.Add(new BufferedInput
        {
            input = _input,
            type = MoveType.Attack,
            frame = FrameClock.Frame,
            time = RhythmStore.Instance.musicTimeMs,
        });
        SetBufferedInputsTime(bufferedInputs, RhythmStore.Instance.musicTimeMs);
        
        OnInputAdded:
        OnAttackInput((_input));
    }

    public void StopMovement()
    {
        movementStoppedThisFrame = true;
    }

    private void LateUpdate()
    {
        if (hasInputThisFrame)
        {
            if (bufferedInputs.Count > 0)
            {
                CharacterMove newMove = MoveMaster.i.GetMove(bufferedInputs);
                if (newMove.priority > 0)
                {
                    if (handler.PlayerIndex == 1)
                    {
                        if(newMove.moveType == MoveType.Movement)
                            if (!movementStoppedThisFrame)
                                EventBus.Emit("p1_move", newMove);
                            else
                                movementStoppedThisFrame = false;
                        else if(newMove.moveType == MoveType.Attack)
                            EventBus.Emit("p1_attack", newMove);
                    }
                    else if (handler.PlayerIndex == 2)
                    {
                        if(newMove.moveType == MoveType.Movement)
                            if (!movementStoppedThisFrame)
                                EventBus.Emit("p2_move", newMove);
                            else
                                movementStoppedThisFrame = false;
                        else if(newMove.moveType == MoveType.Attack)
                            EventBus.Emit("p2_attack", newMove);
                    }
                }
            }
            hasInputThisFrame = false;
        }
        ClearExpiredInputs();
    }

    private InputType GetDirectionalInputType(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        InputType dirInput = InputType.None;

        if (angle is < SIXTEENTH and >= -SIXTEENTH)
            dirInput = InputType.Right;
        else if(angle is >= SIXTEENTH and < SIXTEENTH + EIGHTH)
            dirInput = InputType.RightUp;
        else if (angle is >= SIXTEENTH + EIGHTH and < SIXTEENTH + QUARTER)
            dirInput = InputType.Up;
        else if (angle is >= SIXTEENTH + QUARTER and < HALF - SIXTEENTH)
            dirInput = InputType.LeftUp;
        else if (angle is >= HALF - SIXTEENTH or < -HALF + SIXTEENTH)
            dirInput = InputType.Left;
        else if(angle is >= -HALF + SIXTEENTH and < -SIXTEENTH - QUARTER)
            dirInput = InputType.LeftDown;
        else if(angle is >= -SIXTEENTH - QUARTER and < SIXTEENTH - QUARTER)
            dirInput = InputType.Down;
        else if (angle is >= SIXTEENTH - QUARTER and < -SIXTEENTH)
            dirInput = InputType.RightDown;
        return dirInput;
    }

    private void OnAttackInput(InputType _input)
    {
        switch (handler.PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_attackinput", _input);
                break;
            case 2:
                EventBus.Emit("p2_attackinput", _input);
                break;
            default:
                break;
        }
    }

    private void OnDirectionalInput(InputType _input)
    {
        switch (handler.PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_moveinput", _input);
                break;
            case 2:
                EventBus.Emit("p2_moveinput", _input);
                break;
            default:
                break;
        }
    }
    
    private void SetBufferedInputsTime(List<BufferedInput> inputList, float msTime)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            BufferedInput input = inputList[i];
            input.frame = FrameClock.Frame;
            input.time = msTime;
        }
    }
    
    // private bool IsSimultaneous(BufferedInput _a, BufferedInput _b)
    // {
    //     return Mathf.Abs(_a.frame - _b.frame) <= SIMULTANEOUS_FRAMES;
    // }
    // private void AddValidMoves(int _playerIndex, List<BufferedInput> _dir, List<BufferedInput> _att)
    // {
    //     InputType[] currMove = new InputType[VALID_INPUTS_ARRAY_SIZE];
    //     AttackData attack = new AttackData();
    //
    //     for (int i = 0; i < 3; i++)
    //     {
    //         if (_dir.Count <= i) break;
    //         if (_dir[i].input != InputType.None)
    //         {
    //             currMove[i] = _dir[i].input;
    //         }
    //     }
    //
    //     for (int i = 3; i < 6; i++)
    //     {
    //         if (_att.Count <= i - 3) break;
    //         if (_att[i - 3].input != InputType.None)
    //         {
    //             currMove[i] = _att[i - 3].input;
    //         }
    //     }
    //
    //     attack.inputs = currMove;
    //     attack = GetAttackForInput(ref attack);
    //
    //     if (_playerIndex == 1)
    //     {
    //         p1Attack = attack;
    //     }
    //     else if( _playerIndex == 2)
    //     {
    //         p2Attack = attack;
    //     }
    // }
    // public bool CanAcceptNextInput(int _playerIndex)
    // {
    //     if (_playerIndex == 1)
    //         return p1Attack.inputs != null && p1Attack.frame >= p1Attack.chainStartFrame && p1Attack.frame <= p1Attack.chainEndFrame;
    //     else if (_playerIndex == 2)
    //         return p2Attack.inputs != null && p2Attack.frame >= p2Attack.chainStartFrame && p2Attack.frame <= p2Attack.chainEndFrame;
    //     else return false;
    // }
    // public bool IsExpired(int _playerIndex)
    // {
    //     if (_playerIndex == 1)
    //         return p1Attack.inputs != null && p1Attack.frame > p1Attack.chainEndFrame;
    //     else if (_playerIndex == 2)
    //         return p2Attack.inputs != null && p2Attack.frame > p2Attack.chainEndFrame;
    //     else return false;
    // }
    // #endregion
    //
    // private void Update()
    // {
    //     if (P1Inputs_Attack.Count > 0)
    //     {
    //         tempAttackInputs.Clear();
    //
    //         if (P1Inputs_Attack[0].frame < GameClock.Frame - SIMULTANEOUS_FRAMES)
    //         {
    //             BufferedInput a = P1Inputs_Attack[0];
    //             tempAttackInputs.Add(a);
    //
    //             if (P1Inputs_Attack.Count > 1)
    //             {
    //                 BufferedInput b = P1Inputs_Attack[1];
    //
    //                 if (IsSimultaneous(a, b))
    //                 {
    //                     tempAttackInputs.Add(b);
    //                 }
    //             }
    //
    //             AddValidMoves(1, P1Inputs_Direction, tempAttackInputs);
    //             ConsumeInputs(1, ref P1Inputs_Direction, tempAttackInputs);
    //         }
    //     }
    //     if (P2Inputs_Attack.Count > 0)
    //     {
    //         tempAttackInputs.Clear();
    //
    //         if (P2Inputs_Attack[0].frame < GameClock.Frame - SIMULTANEOUS_FRAMES)
    //         {
    //             BufferedInput a = P2Inputs_Attack[0];
    //             tempAttackInputs.Add(a);
    //
    //             if (P2Inputs_Attack.Count > 1)
    //             {
    //                 BufferedInput b = P2Inputs_Attack[1];
    //
    //                 if (IsSimultaneous(a, b))
    //                 {
    //                     tempAttackInputs.Add(b);
    //                 }
    //             }
    //
    //             AddValidMoves(2, P2Inputs_Direction, tempAttackInputs);
    //             ConsumeInputs(2, ref P2Inputs_Direction, tempAttackInputs);
    //         }
    //     }
    // }
    //
    // private void ConsumeInputs(int _playerIndex, ref List<BufferedInput> _dir, List<BufferedInput> _att)
    // {
    //     _dir.Clear();
    //
    //     if (_playerIndex == 1)
    //     {
    //         foreach (BufferedInput a in _att)
    //         {
    //             P1Inputs_Attack.Remove(a);
    //         }
    //     }
    //     else if(_playerIndex == 2)
    //     {
    //         foreach (BufferedInput a in _att)
    //         {
    //             P2Inputs_Attack.Remove(a);
    //         }
    //     }
    // }
    //
    // private void Start()
    // {
    //     CombatResolver.i.SetInputBuffer(this);
    // }
    //
    // public CombatIntent[] GetPendingIntents()
    // {
    //     CombatIntent p1Intent = new CombatIntent();
    //     CombatIntent p2Intent = new CombatIntent();
    //
    //     p1Intent.player = 1;
    //     p2Intent.player = 2;
    //
    //     p1Intent.attack = p1Attack;
    //     p2Intent.attack = p2Attack;
    //
    //     p1Intent.hitFrame = p1Attack.frame + p1Attack.startupFrames;
    //     p1Intent.endFrame = p1Intent.hitFrame + p1Attack.activeFrames;
    //
    //     return new CombatIntent[] { p1Intent, p2Intent };
    // }
    // AttackData GetAttackForInput(ref AttackData _attack)
    // {
    //     _attack.action = GetActionForAttack(_attack);
    //     _attack.frame = GameClock.Frame;
    //     _attack.startupFrames = 5;
    //     _attack.activeFrames = 10;
    //     _attack.recoveryFrames = 5;
    //     _attack.chainStartFrame = 12;
    //     _attack.chainEndFrame = 20;
    //     return _attack;
    // }
    // CombatActionType GetActionForAttack(AttackData _attack)
    // {
    //     if (_attack.inputs[3] == InputType.LightAtt)
    //         return CombatActionType.LightAttack;
    //     else return CombatActionType.None;
    // }
}

public struct BufferedInput
{
    public InputType input;
    public MoveType type;
    public int frame;
    public float time;
}

