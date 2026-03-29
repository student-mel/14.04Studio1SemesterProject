using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public PlayerHealth player1;
    public PlayerHealth player2;

    public int player1Rounds;
    public int player2Rounds;

    public int roundsToWin = 2;

    private bool roundActive = false;

    void Start()
    {
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        yield return new WaitForSeconds(1f);

        roundActive = true;

        AudioManager.Instance?.PlayRoundStart();
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

    void EndRound(PlayerHealth winner)
    {
        roundActive = false;

        if (winner == player1)
            player1Rounds++;
        else
            player2Rounds++;

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
