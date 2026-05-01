using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RhythmMetronome : MonoBehaviour
{
    private float _bpm;
    
    private float _beatDurationMs;

    private float _nextBeatPosition;
    private float _nextClickPosition;

    public float errorMarginMs = 40f;
    private float _activeBeatStartPos, _activeBeatEndPos;
    private int _enterBeatIndex = 0;
    private int _exitBeatIndex = 0;

    private float _currMusicTime;

    private int _currBeatIndex = 0;
    
    AudioSource audioSource;
    [SerializeField]
    private AudioClip clip;
    
    Coroutine MetronomeCoroutine;

    private void Awake()
    {
        audioSource =  GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe("song_started", StartMetronome);
        EventBus.Subscribe("song_stopped", StopMetronome);
    }

    private void StopMetronome(object obj)
    {
        if (MetronomeCoroutine != null)
            StopCoroutine(MetronomeCoroutine);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("song_started", StartMetronome);
        EventBus.Unsubscribe("song_stopped", StopMetronome);
    }

    private void Start()
    {
        _bpm = RhythmStore.Instance.bgm.bpm;
        _beatDurationMs = 60 / _bpm * 1000f;
        RhythmStore.Instance.beatDuration = _beatDurationMs;
        
        Initialise();
    }

    void Initialise()
    {
        //_nextBeatPosition = _beatDurationMs * 0.82f;
        
        _activeBeatStartPos = _nextBeatPosition - errorMarginMs;
        _activeBeatEndPos = _nextBeatPosition + errorMarginMs;
    }

    void StartMetronome(object obj)
    {
        Initialise();

        MetronomeCoroutine = StartCoroutine(MetronomeRoutine());
    }
    

    IEnumerator MetronomeRoutine()
    {
        while (true)
        {
            _currMusicTime = RhythmStore.Instance.musicTimeMs;
            float prevBeatTime = _nextBeatPosition - _beatDurationMs;

            // distance to nearest beat
            float distToPrev = Mathf.Abs(_currMusicTime - prevBeatTime);
            float distToNext = Mathf.Abs(_currMusicTime - _nextBeatPosition);

            float beatOffset = distToPrev < distToNext ? distToPrev : distToNext;

            // off-beat (syncopated)
            float offBeatTime = prevBeatTime + (_beatDurationMs / 2f);
            float offBeatOffset = Mathf.Abs(_currMusicTime - offBeatTime);

            RhythmStore.Instance.beatOffsetMs = beatOffset;
            RhythmStore.Instance.offBeatOffsetMs = offBeatOffset;

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
            // CLICK PLAYS HALF A BEAT EARLIER
            if (_currMusicTime >= _nextClickPosition)
            {
                audioSource.PlayOneShot(clip);
                _nextClickPosition += _beatDurationMs;
            }

            if (_currMusicTime >= _nextBeatPosition)
            {
                _currBeatIndex = (_currBeatIndex + 1) % 4;
                //Debug.Log($"beat = {_currBeatIndex}");
                EventBus.Emit("beat", _currBeatIndex);
                _nextBeatPosition += _beatDurationMs;
                
                //audioSource.PlayOneShot(clip);
                
                RhythmStore.Instance.currentBeatIndex =  _currBeatIndex;
            }
            yield return null;
        }
    }
    
    public float GetOffsetToNearestBeat()
    {
        float musicTime = RhythmStore.Instance.musicTimeMs;

        float prevBeatTime = _nextBeatPosition - _beatDurationMs;
        float nextBeatTime = _nextBeatPosition;

        float distToPrev = musicTime - prevBeatTime;
        float distToNext = musicTime - nextBeatTime;

        if (Mathf.Abs(distToPrev) < Mathf.Abs(distToNext))
            return distToPrev;
        
        return distToNext;
    }
    
    public float GetOffsetToNearestOffBeat()
    {
        float musicTime = RhythmStore.Instance.musicTimeMs;

        float offBeatTime = _nextBeatPosition - (_beatDurationMs / 2f);

        return musicTime - offBeatTime;
    }
}
