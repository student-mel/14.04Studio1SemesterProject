using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public CinemachineCamera cinemachineCamera;

    public float minZoom = 5f;
    public float maxZoom = 12f;

    public float minDistance = 2f;
    public float maxDistance = 15f;

    public float zoomSpeed = 2f;

    void Update()
    {
        if (player1 == null || player2 == null || cinemachineCamera == null)
            return;

        float distance = Vector3.Distance(player1.position, player2.position);

        float targetZoom = Mathf.Lerp(
            minZoom,
            maxZoom,
            Mathf.InverseLerp(minDistance, maxDistance, distance)
        );

        float currentZoom = cinemachineCamera.Lens.OrthographicSize;

        cinemachineCamera.Lens.OrthographicSize =
            Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
    }
}
