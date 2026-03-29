using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MoveUI : MonoBehaviour
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

    [Range(0.0f, 1.0f)] public float Fade;
    private float currFade;

    public event Action OnFadeChanged;

    public float FadeValue
    {
        get => currFade;
        set
        {
            if (currFade == value) return;
            float oldValue = currFade;
            currFade = value;
            OnFadeChanged?.Invoke();
        }
    }

    [Header("Spawn Settings")]

    public Vector3 StartPosition;
    public Vector2 AnchorPosition;

    [Tooltip("Do not touch unless you know what you are doing")]
    [Header("Reference Settings")]

    public string[] spriteNames;

    public GameObject textPrefab;

    private List<TMP_Text> textList = new List<TMP_Text>();
    private List<int> textStack = new List<int>();

    private RectTransform rectTransform;
    private PlayerInputHandler inputHandler;

    private int inputIndex = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        currLength = Length;
        for (int i = 0; i < currLength; i++)
        {
            GameObject textObj = Instantiate(textPrefab, transform);

            textList.Add(textObj.GetComponent<TMP_Text>());
            textList[i].text = "";

            textStack.Add(0);

            UpdateTextPosition();
        }
    }

    private void OnEnable()
    {
        OnLengthChanged += UpdateTextList;
        OnScaleChanged += UpdateTextPosition;
        OnFadeChanged += UpdateTextPosition;
    }
    private void OnDisable()
    {
        OnLengthChanged -= UpdateTextList;
        OnScaleChanged -= UpdateTextPosition;
        OnFadeChanged -= UpdateTextPosition;
    }

    private void Update()
    {
        LengthValue = Length;
        ScaleValue = Scale;
        FadeValue = Fade;
    }

    public void AssignInputHandler(PlayerInputHandler _handler)
    {
        inputHandler = _handler;
    }

    public void AddMoveToQueue(int? attackSpriteIndex, int? dirSpriteIndex)
    {
        string newText = "";

        if (dirSpriteIndex != null)
        {
            newText += $"<sprite name={spriteNames[(int)dirSpriteIndex]}>";
        }
        if(attackSpriteIndex != null)
        {
            if (newText != "")
                newText += "+";
            newText += $"<sprite name={spriteNames[(int)attackSpriteIndex]}>";
        }

        if(newText != "")
        {
            string lastMoveText = "";

            if (Index == 1)
                lastMoveText = textList[0].text.Replace($" {textStack[0]}", "");
            else if (Index == 2)
                lastMoveText = textList[0].text.Replace($"{textStack[0]} ", "");

            if (!lastMoveText.Equals(newText) || inputIndex != inputHandler.inputIndex)
            {
                for (int i = textList.Count - 1; i >= 1; i--)
                {
                    textList[i].text = textList[i - 1].text;
                    textStack[i] = textStack[i - 1];
                }
                textList[0].text = newText;
                textStack[0] = 1;

                inputIndex = inputHandler.inputIndex;
            }
            else
            {
                textStack[0]++;
                if (Index == 1)
                    textList[0].text = newText + $" {textStack[0]}";
                else if (Index == 2)
                    textList[0].text = $"{textStack[0]} " + newText;
            }
        }
    }
    private void UpdateTextPosition()
    {
        for(int i = 0; i < textList.Count; i++)
        {
            Vector3 newPos = new Vector3();

            int lastIndex = (i - 1) >= 0 ? i - 1 : i;

            if (Index == 1)
            {
                newPos =    StartPosition                                   + 
                            (Vector3.up * rectTransform.rect.height / 2)    + 
                            Vector3.down * i * textList[0].fontSize;

                textList[i].alignment = TextAlignmentOptions.MidlineLeft;
            }
            else if (Index == 2)
            {
                newPos =    (Vector3.right * rectTransform.rect.width)      +
                            StartPosition                                   + 
                            (Vector3.up * rectTransform.rect.height / 2)    + 
                            Vector3.down * i * textList[0].fontSize;

                textList[i].alignment = TextAlignmentOptions.MidlineRight;
            }

            textList[i].rectTransform.anchorMin = AnchorPosition;
            textList[i].rectTransform.anchorMax = AnchorPosition;
            textList[i].rectTransform.position = newPos;
            if(i > 0)
                textList[i].fontSize = textList[lastIndex].fontSize * Scale ;

            Color newColor = textList[i].color;
            newColor.a = Mathf.Pow(Fade, i);
            textList[i].color = newColor;
        }
    }
    private void UpdateTextList()
    {
        if(LengthValue > textList.Count)
        {
            int diff = LengthValue - textList.Count;
            for (int i = 0; i < diff; i++)
            {
                GameObject textObj = Instantiate(textPrefab, transform);

                TMP_Text newText = textObj.GetComponent<TMP_Text>();
                newText.text = "";
                textList.Add(newText);

                textStack.Add(0);
            }
            UpdateTextPosition();
        }
        else if(LengthValue < textList.Count)
        {
            int diff = textList.Count - LengthValue;
            for (int i = 0; i < diff; i++)
            {
                TMP_Text textObj = textList[textList.Count - 1 - i];

                textList.Remove(textObj);

                Destroy(textObj.gameObject);
            }
        }
    }
}

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
