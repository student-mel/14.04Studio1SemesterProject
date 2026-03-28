using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    private float _bpm;
    
    private float _beatDurationMs;

    private float _nextBeatPosition;
    
    public float errorMarginMs = 80f;
    private float _activeBeatStartPos, _activeBeatEndPos;
    private int _enterBeatIndex = 0;
    private int _exitBeatIndex = 0;

    private float _currMusicTime;

    private int _currBeatIndex = 0;
    
    AudioSource audioSource;
    [SerializeField]
    private AudioClip clip;

    private void Awake()
    {
        audioSource =  GetComponent<AudioSource>();
    }

    private void Start()
    {
        _bpm = RhythmStore.Instance.bgm.bpm;
        _beatDurationMs = 60 / _bpm * 1000f;
        _nextBeatPosition = _beatDurationMs;
        
        _activeBeatStartPos = _nextBeatPosition - errorMarginMs;
        _activeBeatEndPos = _nextBeatPosition + errorMarginMs;

        StartCoroutine(MetronomeRoutine());
    }

    IEnumerator MetronomeRoutine()
    {
        while (true)
        {
            _currMusicTime = RhythmStore.Instance.musicTimeMs;

            if (_currMusicTime >= _activeBeatStartPos)
            {
                _enterBeatIndex = (_currBeatIndex + 1) % 4;
                _activeBeatStartPos += _beatDurationMs;
                RhythmStore.Instance.activeBeat =  _enterBeatIndex;
            }

            if (_currMusicTime >= _activeBeatEndPos)
            {
                _exitBeatIndex = _currBeatIndex;
                _activeBeatEndPos += _beatDurationMs;
                RhythmStore.Instance.activeBeat =  -1;
            }
            
            if (_currMusicTime >= _nextBeatPosition)
            {
                _currBeatIndex = (_currBeatIndex + 1) % 4;
                Debug.Log($"beat = {_currBeatIndex}");
                EventBus.Emit("beat", _currBeatIndex);
                _nextBeatPosition += _beatDurationMs;
                
                audioSource.PlayOneShot(clip);
                
                RhythmStore.Instance.currentBeatIndex =  _currBeatIndex;
            }
            yield return null;
        }
    }
}
