using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestBeatClock : MonoBehaviour
{
    public static TestClock Clock;
    public static float Interval = 0.5f;

    public static float NextTick => Time.time + Interval;

    public Image BeatCue;
    private Color original;

    private void Awake()
    {
        original = BeatCue.color;
        CreateClock();
    }
    public static void CreateClock()
    {
        if (Clock == null)
        {
            Clock = new TestClock(Interval);
        }
    }
    private void Update()
    {
        Clock.Update(Time.deltaTime);
    }

    private void OnEnable()
    {
        Clock.CustomUpdate += Blink;
    }
    private void OnDisable()
    {
        Clock.CustomUpdate -= Blink;
    }
    private void Blink()
    {
        StartCoroutine(BlinkToBeat(NextTick));
    }

    IEnumerator BlinkToBeat(float _nextTick)
    {
        yield return new WaitWhile(() => Time.time < _nextTick - Interval * 0.2f);

        BeatCue.color = Color.crimson;
        yield return new WaitForSeconds(Interval * 0.4f);
        BeatCue.color = original;
    }
}
