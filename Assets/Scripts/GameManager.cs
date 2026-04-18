using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Health player1;
    public Health player2;

    public int player1Rounds;
    public int player2Rounds;

    public int roundsToWin = 2;
    public roundCounters player1UI;
    public roundCounters player2UI;

    public TimerUI timerUI;
    public CountdownUI countdownUI;

    private bool roundActive = false;
    public static bool InputLocked = true;

    void Start()
    {
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        roundActive = false;
        InputLocked = true;

        AudioManager.Instance?.PlayPreRound();

        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(countdownUI.PlayCountdown());

        AudioManager.Instance?.PlayMusic();

        timerUI?.StartTimer();

        InputLocked = false;
        roundActive = true;
    }

    void Update()
    {
        if (!roundActive) return;



        if (player1.currentHealth <= 0)
        {
            EndRound(player2);
        }
        else if (player2.currentHealth <= 0)
        {
            EndRound(player1);
        }
    }

    void EndRound(Health winner)
    {
        roundActive = false;
        InputLocked = true;

        timerUI?.StopTimer();

        if (winner == player1)
        {
            player1Rounds++;
            player1UI?.WinRound();
        }
        else
        {
            player2Rounds++;
            player2UI?.WinRound();
        }

        AudioManager.Instance?.PlayKO();

        StartCoroutine(HandleRoundEnd());
    }

    IEnumerator HandleRoundEnd()
    {
        yield return new WaitForSeconds(2f);

        // Wincon check
        if (player1Rounds >= roundsToWin || player2Rounds >= roundsToWin)
        {
            Debug.Log("Match Over");
            // Scene reload - change for when we have scenes
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            ResetRound();
            StartCoroutine(StartRound());
        }
    }

    void ResetRound()
    {
        player1.ResetPlayer();
        player2.ResetPlayer();
    }
}
