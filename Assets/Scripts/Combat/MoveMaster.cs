using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveMaster : MonoBehaviour
{
    public static MoveMaster i;
    private List<Move> moveList;
    [SerializeField]private MoveLibrary Library;

    
    private void Awake()
    {
        if (i == null) i = this;
        else if (i != this) Destroy(gameObject);

        moveList = new List<Move>(Library.MoveList);
    }

    public void GetMove(List<BufferedInput> inputs)
    {
        foreach (Move move in moveList)
        {
            move.priority = 0;
        }
        if (inputs.Count > 0)
        {
            foreach (Move move in moveList)
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
                        priority += inputs[p].priority;
                        stringLength++;
                        playerInputStringIndex = p;
                        
                        if(stringLength == inputString.Count) break;
                    }
                }
                if(stringLength == inputString.Count)
                    move.priority = priority;
            }
            moveList = moveList.OrderBy(i => i.priority).ToList();
            string debugString = "\n";
            for (int i = moveList.Count - 1; i > 0; i--)
            {
                debugString += $"{moveList[i].Name}: {moveList[i].priority}\n";
            }
            Debug.Log(debugString);

            Move newMove = moveList[^1];
            if (newMove.priority > 0)
            {
                if(newMove.moveType == MoveType.Attack)
                    EventBus.Emit("attack", newMove);
                else if(newMove.moveType == MoveType.Movement)
                    EventBus.Emit("move", newMove);
            }
        }
    }
}
