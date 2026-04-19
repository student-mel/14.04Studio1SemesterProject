using UnityEngine;

public class PracticeModeManager : MonoBehaviour
{
    public Health player1;
    public Health player2;

    public bool infiniteHealth = true;

    void Update()
    {
        if (infiniteHealth)
        {
            if (player1.currentHealth <= 0)
                player1.ResetPlayer();

            if (player2.currentHealth <= 0)
                player2.ResetPlayer();
        }
    }
}
