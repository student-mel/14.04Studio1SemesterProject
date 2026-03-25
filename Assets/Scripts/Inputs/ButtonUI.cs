using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Image[] Buttons;

    public Color Default;
    public Color Pressed;

    public int index = 1;
    private PlayerInputHandler player;

    private Transform playerTransform;

    private void Start()
    {
        PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
        player = players.FirstOrDefault(p => p.PlayerIndex == index);

        if(player != null)
        {
            player.MoveEvent += MoveButtonPressed;
            player.MoveEndedEvent += MoveButtonUnpressed;
            player.AttackEvent += AttackButtonPressed;
            player.AttackEndedEvent += AttackButtonUnpressed;
        }

        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");

        if( playerObjs.Length <= 0)
        {
            Debug.Log("No player found in scene");
            return;
        }

        if(index == 1)
        {
            playerTransform = playerObjs[0].transform.position.x < playerObjs[1].transform.position.x ? playerObjs[0].transform : playerObjs[1].transform;
        }
        else if (index == 2)
        {
            playerTransform = playerObjs[0].transform.position.x > playerObjs[1].transform.position.x ? playerObjs[0].transform : playerObjs[1].transform;
        }
        else
        {
            Debug.LogWarning("Player index under ButtonUI can only be 1 or 2");
        }

        if(playerTransform != null)
        {
            transform.parent = playerTransform;
            transform.localPosition = playerTransform.localPosition;
            transform.position += Vector3.up * 1.2f;
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
