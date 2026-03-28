using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmBarPanelHandler : MonoBehaviour
{
    [SerializeField]
    private RectTransform centerImg;

    [Header("Beats")]
    public RectTransform parent1,  parent2;
    private readonly List<RectTransform> _activeBeats = new();
    [SerializeField] float startX = 140f;
    [SerializeField] float targetX = -115f;
    [SerializeField] int groupSize = 3;

    private float _musicTime;
    private float _beatDuration;
    
    private void OnEnable()
    {
        EventBus.Subscribe("beat", BeatPulse);
    }

    private void Start()
    {
        AddBeatsToList(parent1);
        AddBeatsToList(parent2);
    }

    private void Update()
    {
        UpdateBeats();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("beat", BeatPulse);
    }

    private void BeatPulse(object obj)
    {
        StartCoroutine(PulseImage(1.2f, 0.25f, (int)obj));
    }

    IEnumerator PulseImage(float scaleSize, float pulseDur, int beatIndex)
    {
        float scaleAmt = beatIndex == 0 ? scaleSize * 1.2f : scaleSize;
        Vector3 size = Vector3.one *  scaleAmt;
        centerImg.transform.localScale = size;

        float start = scaleAmt;
        
        float t = 0f;
        while (t < 1)
        {
            scaleAmt = Mathf.Lerp(start, 1f, t);
            centerImg.localScale = Vector3.one * scaleAmt;
            t += Time.deltaTime/pulseDur;
            yield return null;
        }

        centerImg.localScale = Vector3.one;
    }
    
    void UpdateBeats()
    {
        _musicTime = RhythmStore.Instance.musicTimeMs;
        _beatDuration = RhythmStore.Instance.beatDuration;

        float loopDuration = _beatDuration * groupSize;
        float loopTime = _musicTime % loopDuration;

        for (int i = 0; i < _activeBeats.Count; i++)
        {
            RectTransform beat = _activeBeats[i];

            int groupIndex = i % groupSize;

            float beatOffsetTime = groupIndex * _beatDuration;

            float t = (loopTime + beatOffsetTime) % loopDuration;

            float normalized = t / loopDuration;

            float x = Mathf.Lerp(startX, targetX, normalized);

            Vector2 pos = beat.anchoredPosition;
            pos.x = x;
            beat.anchoredPosition = pos;
        }
    }

    void AddBeatsToList(RectTransform rt)
    {
        for (int i = 0; i < rt.childCount; i++)
        {
            _activeBeats.Add(rt.GetChild(i).GetComponent<RectTransform>());
        }
    }
}
