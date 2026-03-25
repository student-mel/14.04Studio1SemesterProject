using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerDeviceHandler : MonoBehaviour
{
    public PlayerInput prefab;
    public InputActionAsset action;

    public PlayerInput Player1 { get; private set; }
    public PlayerInput Player2 { get; private set; }

    public InputAction Player1JoinKey;
    public InputAction Player2JoinKey;

    HashSet<InputDevice> claimedDevices = new HashSet<InputDevice>();

    public bool debug = false;

    private void Awake()
    {
        JoinKeyboardP1();
        JoinKeyboardP2();
    }

    private void OnEnable()
    {
        InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);

        Player1JoinKey.Enable();
        Player2JoinKey.Enable();
    }
    private void OnDisable()
    {
        Player1JoinKey.Disable();
        Player2JoinKey.Disable();
    }

    private void OnAnyButtonPressed(InputControl _control)
    {
        InputDevice device = _control.device;

        if (_control.Equals(Player1JoinKey.controls[0])) { JoinKeyboardP1(); }
        else if (_control.Equals(Player2JoinKey.controls[0])) { JoinKeyboardP2(); }

        if (device is not Gamepad) return;

        if(claimedDevices.Contains(device)) return;

        JoinWithGamePad(device);
    }

    private void JoinWithGamePad(InputDevice _device)
    {
        PlayerInput input = Player1;

        if (Player1.currentControlScheme.Equals("Gamepad"))
            input = Player2;
        else if (Player2.currentControlScheme.Equals("Gamepad") && input == Player2)
        {
            if(debug)
                Debug.LogWarning("Only supports up to 2 Gamepads");
            return;
        }

        bool isP1 = input == Player1;

        if (isP1) keyboardP1Joined = false;
        else keyboardP2Joined = false;

        claimedDevices.Add(_device);

        input.SwitchCurrentControlScheme("Gamepad", _device);

        if(debug)
            Debug.Log($"Player {(isP1? "1" : "2")} joined with {_device.displayName}");
    }

    bool keyboardP1Joined = false;
    bool keyboardP2Joined = false;

    private void JoinKeyboardP1()
    {
        if (keyboardP1Joined) return;

        keyboardP1Joined = true;

        if (Player1 == null)
        {
            GameObject player = Instantiate(prefab.gameObject, transform);
            Player1 = player.GetComponent<PlayerInput>();
        }

        Player1.enabled = false;
        Player1.actions = action;

        Player1.defaultActionMap = "Gameplay";
        Player1.actions.FindActionMap("Gameplay").Enable();

        Player1.defaultControlScheme = "Keyboard_P1";

        Player1.enabled = true;

        Player1.ActivateInput();

        if (Player1.currentControlScheme != null)
            if (Player1.currentControlScheme.Equals("Gamepad"))
                claimedDevices.Remove(Player1.devices[0]);

        Player1.SwitchCurrentControlScheme("Keyboard_P1", Keyboard.current);

        if (debug)
            Debug.Log($"Player 1 joined with {Keyboard.current}");
    }
    private void JoinKeyboardP2()
    {
        if (keyboardP2Joined) return;

        keyboardP2Joined = true;

        if (Player2 == null)
        {
            GameObject player = Instantiate(prefab.gameObject, transform);
            Player2 = player.GetComponent<PlayerInput>();
        }

        Player2.enabled = false;
        Player2.actions = action;

        Player2.defaultActionMap = "Gameplay";
        Player2.actions.FindActionMap("Gameplay").Enable();

        Player2.defaultControlScheme = "Keyboard_P2";

        Player2.enabled = true;

        Player2.ActivateInput();

        if (Player2.currentControlScheme != null)
            if (Player2.currentControlScheme.Equals("Gamepad"))
                claimedDevices.Remove(Player2.devices[0]);

        Player2.SwitchCurrentControlScheme("Keyboard_P2", Keyboard.current);

        if (debug)
            Debug.Log($"Player 2 joined with {Keyboard.current}");
    }
}
