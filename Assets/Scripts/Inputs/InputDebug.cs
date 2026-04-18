using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InputDebug : MonoBehaviour
{
    [Header("Player Setting")]

    [Range(1, 2)] public int Index;

    [Header("UI Settings")]

    [Range(1, 25)] public int Length;
    private int currLength;

    public event Action OnLengthChanged;

    public int LengthValue
    {
        get => currLength;
        set
        {
            if (currLength == value) return;
            int oldValue = currLength;
            currLength = value;
            OnLengthChanged?.Invoke();
        }
    }

    [Range(0.5f, 1.0f)] public float Scale;
    private float currScale;

    public event Action OnScaleChanged;

    public float ScaleValue
    {
        get => currScale;
        set
        {
            if (currScale == value) return;
            float oldValue = currScale;
            currScale = value;
            OnScaleChanged?.Invoke();
        }
    }

    [Range(0.0f, 1.0f)] public float Opacity;
    private float currOpacity;

    public event Action OnOpacityChanged;

    public float OpacityValue
    {
        get => currOpacity;
        set
        {
            if (currOpacity == value) return;
            float oldValue = currOpacity;
            currOpacity = value;
            OnOpacityChanged?.Invoke();
        }
    }

    const int MAX_DATA_SIZE = 25;
    private float prevFrame;
    
    [Tooltip("Do not touch unless you know what you are doing")]
    [Header("Reference Settings")]

    public GameObject textPrefab;
    
    private List<TMP_Text> tmpTextList = new List<TMP_Text>();
    private List<TMP_Text> tmpCountList = new List<TMP_Text>();
    
    private List<string> textList = new List<string>();
    private List<int> countList = new List<int>();

    public Transform ButtonsTextParent;
    public Transform CounterTextParent;
    
    public string[] InputTextNames;

    private void Awake()
    {
        for (int i = 0; i < MAX_DATA_SIZE; i++)
        {
            textList.Add("");
            countList.Add(0);
        }
        
        currLength = Length;
        for (int i = 0; i < currLength; i++)
        {
            GameObject textObj = Instantiate(textPrefab, ButtonsTextParent);

            tmpTextList.Add(textObj.GetComponent<TMP_Text>());
            tmpTextList[i].text = textList[i];
            
            GameObject countObj = Instantiate(textPrefab, CounterTextParent);
            tmpCountList.Add(countObj.GetComponent<TMP_Text>());
            tmpCountList[i].text = "";

            UpdateTextPosition();
        }
    }

    private void OnEnable()
    {
        OnLengthChanged += UpdateTextList;
        OnScaleChanged += UpdateTextPosition;
        OnOpacityChanged += UpdateTextPosition;

        if (Index == 1)
        {
            EventBus.Subscribe("p1_move", AddMoveInputs);
            EventBus.Subscribe("p1_attack", AddMoveInputs);
        }
        else if (Index == 2)
        {
            EventBus.Subscribe("p2_move", AddMoveInputs);
            EventBus.Subscribe("p2_attack", AddMoveInputs);
        }
    }
    private void OnDisable()
    {
        OnLengthChanged -= UpdateTextList;
        OnScaleChanged -= UpdateTextPosition;
        OnOpacityChanged -= UpdateTextPosition;
        
        if (Index == 1)
        {
            EventBus.Unsubscribe("p1_move", AddMoveInputs);
            EventBus.Unsubscribe("p1_attack", AddMoveInputs);
        }
        else if (Index == 2)
        {
            EventBus.Unsubscribe("p2_move", AddMoveInputs);
            EventBus.Unsubscribe("p2_attack", AddMoveInputs);
        }
    }

    private void Update()
    {
        LengthValue = Length;
        ScaleValue = Scale;
        OpacityValue = Opacity;
    }

    private void AddMoveInputs(object move)
    {
        Move newMove = (Move)move;
        string newButtons = "";

        foreach (InputType t in newMove.moveString)
        {
            newButtons += $"<sprite name={InputTextNames[(int)t]}>";
        }
        
        float time = Time.time;
        
        if (newButtons == textList[0] && Math.Abs(time - (prevFrame + Time.deltaTime)) < 0.001f)
        {
            countList[0]++;
            tmpCountList[0].text = countList[0].ToString();
        }
        else
        {
            for (int i = MAX_DATA_SIZE - 1; i >= 1; i--)
            {
                textList[i] = textList[i - 1];
                countList[i] = countList[i - 1];
            }
            textList[0] = newButtons;
            countList[0] = 1;
            
            for (int i = 0; i < tmpTextList.Count; i++)
            {
                tmpTextList[i].text = textList[i];
                if(countList[i] > 0)
                    tmpCountList[i].text = countList[i].ToString();
            }
        }
        prevFrame = time;
    }
    private void UpdateTextPosition()
    {
        for(int i = 0; i < tmpTextList.Count; i++)
        {
            //int lastIndex = (i - 1) >= 0 ? i - 1 : i;

            if (Index == 1)
            {
                tmpTextList[i].alignment = TextAlignmentOptions.MidlineLeft;
                tmpCountList[i].alignment = TextAlignmentOptions.MidlineRight;
            }
            else if (Index == 2)
            {   
                tmpTextList[i].alignment = TextAlignmentOptions.MidlineRight;
                tmpCountList[i].alignment = TextAlignmentOptions.MidlineLeft;
            }

            //if (i > 0) { tmpTextList[i].fontSize = tmpTextList[lastIndex].fontSize * Scale; }
            tmpTextList[i].fontSize = 30f * Scale;
            tmpCountList[i].fontSize = 30f * Scale;
            
            Color newColor = tmpTextList[i].color;
            //newColor.a = Mathf.Pow(Fade, i);
            newColor.a = Opacity;
            tmpTextList[i].color = newColor;
            
            newColor = tmpCountList[i].color;
            newColor.a = Opacity;
            tmpCountList[i].color = newColor;
        }
    }
    private void UpdateTextList()
    {
        if(LengthValue > tmpTextList.Count)
        {
            int diff = LengthValue - tmpTextList.Count;
            for (int i = 0; i < diff; i++)
            {
                GameObject textObj = Instantiate(textPrefab, ButtonsTextParent);

                TMP_Text newText = textObj.GetComponent<TMP_Text>();
                tmpTextList.Add(newText);
                int index = tmpTextList.IndexOf(newText);
                tmpTextList[index].text = textList[index];

                textObj = Instantiate(textPrefab, CounterTextParent);

                newText = textObj.GetComponent<TMP_Text>();
                tmpCountList.Add(newText);
                index = tmpCountList.IndexOf(newText);
                tmpCountList[index].text = countList[index].ToString();
            }
            UpdateTextPosition();
        }
        else if(LengthValue < tmpTextList.Count)
        {
            int diff = tmpTextList.Count - LengthValue;
            for (int i = 0; i < diff; i++)
            {
                TMP_Text textObj = tmpTextList[tmpTextList.Count - 1 - i];
                tmpTextList.Remove(textObj);
                Destroy(textObj.gameObject);
                
                TMP_Text textObj2 = tmpCountList[tmpCountList.Count - 1 - i];
                tmpCountList.Remove(textObj2);
                Destroy(textObj2.gameObject);
            }
        }
    }
}

/*
[CustomEditor (typeof(MoveUI))]
public class MoveUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MoveUI moveUI = (MoveUI)target;

        GUILayout.Space(10);

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);

        if(GUILayout.Button("Add Move"))
        {
            moveUI.AddMoveToQueue(UnityEngine.Random.Range(0, 2), 2);
        }

        EditorGUI.EndDisabledGroup();
    }
}
*/
