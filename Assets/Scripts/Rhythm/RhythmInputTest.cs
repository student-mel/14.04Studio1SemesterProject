using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmInputTest : MonoBehaviour
{
    [SerializeField] private int playerIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EventBus.Emit("action", playerIndex);
            Debug.Log("Space pressed -> action emitted");
        }
        
    }
}
