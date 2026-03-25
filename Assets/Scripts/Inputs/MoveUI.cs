using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;

public class MoveUI : MonoBehaviour
{
    public int Index;
    public int Length;
    [Range(0.5f, 1.0f)] public float Scale;
    public Vector3 StartPosition;
    public Vector2 AnchorPosition;

    public string[] spriteNames;

    public GameObject textPrefab;

    private List<TMP_Text> queue = new List<TMP_Text>();
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void AddMoveToQueue(int? attackSpriteIndex, int? dirSpriteIndex)
    {
        GameObject textObj = Instantiate(textPrefab, transform);
        textObj.transform.position = StartPosition;
        TMP_Text text = textObj.GetComponent<TMP_Text>();

        text.text = "";
        if (dirSpriteIndex != null)
        {
            text.text += $"<sprite name={spriteNames[(int)dirSpriteIndex]}>";
        }
        if(attackSpriteIndex != null)
        {
            if (text.text != "")
                text.text += "+";
            text.text += $"<sprite name={spriteNames[(int)attackSpriteIndex]}>";
        }

        if(text.text != "")
            queue.Insert(0, text);
        if(queue.Count > Length)
        {
            Destroy(queue[queue.Count - 1].gameObject);
            queue.RemoveAt(queue.Count - 1);
        }
        UpdateTextPosition();
    }
    private void UpdateTextPosition()
    {
        for(int i = 0; i < queue.Count; i++)
        {
            Vector3 newPos = new Vector3();
            float newHeight;
            float newWidth;

            int lastIndex = (i - 1) >= 0 ? i - 1 : i;

            newHeight = queue[lastIndex].rectTransform.rect.height * Scale;
            newWidth = queue[lastIndex].rectTransform.rect.width * Scale;

            if (Index == 1)
            {
                newPos =    StartPosition                                   + 
                            (Vector3.up * rectTransform.rect.height / 2)    + 
                            Vector3.down * i * queue[i].rectTransform.rect.height;

                queue[i].alignment = TextAlignmentOptions.MidlineLeft;
            }
            else if (Index == 2)
            {
                newPos =    (Vector3.right * rectTransform.rect.width)      +
                            StartPosition                                   + 
                            (Vector3.up * rectTransform.rect.height / 2)    + 
                            Vector3.down * i * queue[i].rectTransform.rect.height;

                queue[i].alignment = TextAlignmentOptions.MidlineRight;
            }

            Debug.Log(newPos);
            queue[i].rectTransform.anchorMin = AnchorPosition;
            queue[i].rectTransform.anchorMax = AnchorPosition;
            queue[i].rectTransform.position = newPos;
            queue[i].rectTransform.rect.Set(newPos.x, newPos.y, newWidth, newHeight);
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
            moveUI.AddMoveToQueue(Random.Range(0, 2), 2);
        }

        EditorGUI.EndDisabledGroup();
    }
}
