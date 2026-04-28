using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    public float minX = -8f;
    public float maxX = 8f;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
