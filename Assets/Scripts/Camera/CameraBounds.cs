using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Transform leftBoundary;
    public Transform rightBoundary;

    public PlayerBounds player1;
    public PlayerBounds player2;

    void Start()
    {
        float min = leftBoundary.position.x;
        float max = rightBoundary.position.x;

        player1.minX = min;
        player1.maxX = max;

        player2.minX = min;
        player2.maxX = max;
    }
}
