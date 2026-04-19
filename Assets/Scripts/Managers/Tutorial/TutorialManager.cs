using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Health player1;
    public Health player2;

    public float resetDelay = 3f;

    private bool resetting = false;

    void Update()
    {
        if (resetting) return;

        if (player1.currentHealth <= 0 || player2.currentHealth <= 0)
        {
            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        resetting = true;

        yield return new WaitForSeconds(resetDelay);

        player1.ResetPlayer();
        player2.ResetPlayer();

        resetting = false;
    }
}
