using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveMaster : MonoBehaviour
{
    public static MoveMaster i;
    private List<CharacterMove> moveList;
    [SerializeField]private MoveLibrary Library;
    
    public bool debug = false;
    
    private void Awake()
    {
        if (i == null) i = this;
        else if (i != this) Destroy(gameObject);

        moveList = new List<CharacterMove>(Library.MoveList);
    }

    public CharacterMove GetMove(List<BufferedInput> inputs)
    {
        foreach (CharacterMove move in moveList)
        {
            move.priority = 0;
        }
        if (inputs.Count > 0)
        {
            foreach (CharacterMove move in moveList)
            {
                int priority = 0;
                int stringLength = 0;
                int playerInputStringIndex = inputs.Count;
                List<InputType> inputString = new List<InputType>(move.moveString);
                
                MoveStringLoop:
                for(int i = inputString.Count - 1; i >= 0; i--)
                {
                    if (playerInputStringIndex - 1 < 0) break;
                    
                    int p = playerInputStringIndex - 1;

                    if (inputString[i] != inputs[p].input)
                    {
                        priority = 0;
                        stringLength = 0;
                        playerInputStringIndex = p;
                        goto MoveStringLoop;
                    }
                    else
                    {
                        priority += playerInputStringIndex;
                        stringLength++;
                        playerInputStringIndex = p;

                        if (stringLength == inputString.Count)
                        {
                            move.priority = priority;
                            break;
                        }
                    }
                }
            }
            moveList = moveList.OrderBy(i => i.priority).ToList();
            if (debug)
            {
                string debugString = "\n";
                foreach (BufferedInput input in inputs)
                {
                    debugString += input.input;
                    debugString += "\n";
                }
                for (int i = moveList.Count - 1; i >= 0; i--)
                {
                    debugString += $"{moveList[i].Name}: {moveList[i].priority}\n";
                }
                Debug.Log(debugString);
            }
        }
        CharacterMove newMove = moveList[^1];
        return newMove;
    }
}
