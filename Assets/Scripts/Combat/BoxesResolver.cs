using UnityEngine;

public class BoxesResolver : MonoBehaviour
{
    public static BoxesResolver i;

    private void Awake()
    {
        if (i == null) i = this;
        else if(i != this) Destroy(gameObject);
    }

    public BoxData PresetData;
    
    public PlayerBoxes player1;
    public PlayerBoxes player2;

    private void OnEnable()
    {
        EventBus.Subscribe("fixed_game_update", FixedGameUpdate);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe("fixed_game_update", FixedGameUpdate);
    }

    private void FixedGameUpdate(object frame)
    {
        CheckProximity();
        CheckOverlaps();
    }
    
    private void CheckProximity()
    {
        Boxes p1Boxes = player1.ToWorld();
        Boxes p2Boxes = player2.ToWorld();
        
        if(p1Boxes == null || p2Boxes == null) return;

        if (p1Boxes.proximityboxes != null && p2Boxes.hurtboxes != null)
        {
            if(p1Boxes.proximityboxes.Length > 0 && p2Boxes.hurtboxes.Length > 0)
                foreach (Box proximitybox in p1Boxes.proximityboxes)
                    foreach (Box hurtbox in p2Boxes.hurtboxes)
                    {
                        if (Overlaps(proximitybox, hurtbox))
                        {
                            EventBus.Emit("p2_anticipate", player1.currMove.Name);
                            //Debug.Log("p2_anticipate");
                            goto Next;
                        }
                    }
        }
        EventBus.Emit("p2_anticipate_cancel", null);
        Next:
        if (p1Boxes.hurtboxes != null && p2Boxes.proximityboxes != null)
        {
            if(p1Boxes.hurtboxes.Length > 0 && p2Boxes.proximityboxes.Length > 0)
                foreach (Box proximitybox in p2Boxes.proximityboxes)
                    foreach (Box hurtbox in p1Boxes.hurtboxes)
                    {
                        if (Overlaps(proximitybox, hurtbox))
                        {
                            EventBus.Emit("p1_anticipate", player2.currMove.Name);
                            //Debug.Log("p1_anticipate");
                            return;
                        }
                    }
        }
        EventBus.Emit("p1_anticipate_cancel", null);
    }
    private void CheckOverlaps()
    {
        Boxes p1Boxes = player1.ToWorld();
        Boxes p2Boxes = player2.ToWorld();
        
        if(p1Boxes == null || p2Boxes == null) return;
        
        if(p1Boxes.hitboxes != null && p2Boxes.hurtboxes != null)
            if(p1Boxes.hitboxes.Length > 0 && p2Boxes.hurtboxes.Length > 0)
                foreach (Box hitbox in p1Boxes.hitboxes)
                    foreach (Box hurtbox in p2Boxes.hurtboxes)
                    {
                        if(Overlaps(hitbox,  hurtbox))
                            if (player1.activeHits > 0)
                            {
                                player1.SetActiveHits(player1.activeHits - 1);
                                EventBus.Emit("p2_hurt", player1.currMove);
                                //Debug.Log(player1.currMove.Name);
                                goto Next;
                            }
                    }
        Next:
        if(p1Boxes.hurtboxes != null && p2Boxes.hitboxes != null)
            if(p1Boxes.hurtboxes.Length > 0 && p2Boxes.hitboxes.Length > 0)
                foreach (Box hitbox in p2Boxes.hitboxes)
                    foreach (Box hurtbox in p1Boxes.hurtboxes)
                    {
                        if(Overlaps(hitbox,  hurtbox))
                            if (player2.activeHits > 0)
                            {
                                player2.SetActiveHits(player2.activeHits - 1);
                                EventBus.Emit("p1_hurt", player2.currMove);
                                //Debug.Log(player2.currMove.Name);
                                return;
                            }
                    }
    }

    private bool Overlaps(Box a, Box b)
    {
        return Mathf.Abs(a.center.x - b.center.x) < (a.size.x + b.size.x) * 0.5f &&
               Mathf.Abs(a.center.y - b.center.y) < (a.size.y + b.size.y) * 0.5f;
    }
}
