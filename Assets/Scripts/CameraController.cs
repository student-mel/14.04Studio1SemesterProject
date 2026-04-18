using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public CinemachineCamera virtualCamera;

    [Header("Zoom Settings")]
    public float minZoom = 4f;   
    public float maxZoom = 10f;  
    public float maxDistance = 15f;

    public float zoomSpeed = 5f;

    void Update()
    {
        if (player1 == null || player2 == null) return;

        float distance = Vector2.Distance(player1.position, player2.position);

        float t = Mathf.Clamp01(distance / maxDistance);

        float targetZoom = Mathf.Lerp(minZoom, maxZoom, t);

        float currentZoom = virtualCamera.Lens.OrthographicSize;
        float newZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);

        virtualCamera.Lens.OrthographicSize = newZoom;
    }
}
