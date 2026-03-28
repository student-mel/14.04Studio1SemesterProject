using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestBeatClock : MonoBehaviour
{
    public static TestClock Clock;
    public static float Interval = 0.5f;

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
        StartCoroutine(BlinkToBeat());
    }
    IEnumerator BlinkToBeat()
    {
        BeatCue.color = Color.crimson;
        yield return new WaitForSeconds(0.05f);
        BeatCue.color = original;
    }
}
