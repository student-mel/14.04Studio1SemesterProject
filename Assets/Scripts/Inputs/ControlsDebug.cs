using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlsDebug : MonoBehaviour
{
    public Image[] Buttons;

    public Color Default;
    public Color Pressed;

    public int index = 1;
    private PlayerInputHandler player;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (player == null)
        {
            PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
            player = players.FirstOrDefault(p => p.PlayerIndex == index);

            player.MoveEvent += MoveButtonPressed;
            player.MoveEndedEvent += MoveButtonUnpressed;
            player.AttackEvent += AttackButtonPressed;
            player.AttackEndedEvent += AttackButtonUnpressed;
        }
    }

    private void OnEnable()
    {
        if(player != null)
        {
            player.MoveEvent += MoveButtonPressed;
            player.MoveEndedEvent += MoveButtonUnpressed;
            player.AttackEvent += AttackButtonPressed;
            player.AttackEndedEvent += AttackButtonUnpressed;
        }
    }
    private void OnDisable()
    {
        if(player != null)
        {
            player.MoveEvent -= MoveButtonPressed;
            player.MoveEndedEvent -= MoveButtonUnpressed;
            player.AttackEvent -= AttackButtonPressed;
            player.AttackEndedEvent -= AttackButtonUnpressed;
        }
    }

    private void OnButtonPressed(InputType input)
    {
        int i = (int)input;
        Buttons[i].color = Pressed;
    }

    private void OnButtonReleased(InputType input)
    {
        int i = (int)input;
        Buttons[i].color = Default;
    }

    public void MoveButtonPressed(Vector2 _input)
    {
        if(_input.x < -0.1)
        {
            Buttons[0].color = Pressed;
            Buttons[1].color = Default;
        }
        else if(_input.x > 0.1)
        {
            Buttons[0].color = Default;
            Buttons[1].color = Pressed;
        }
    }
    public void MoveButtonUnpressed()
    {
        Buttons[0].color = Default;
        Buttons[1].color = Default;
    }

    public void AttackButtonPressed()
    {
        Buttons[2].color = Pressed;
    }
    public void AttackButtonUnpressed()
    {
        Buttons[2].color = Default;
    }
}
