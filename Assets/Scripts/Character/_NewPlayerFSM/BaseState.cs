
public class BaseState
{
    protected PlayerBehaviour pb;
    protected StateMachine fsm;

    public BaseState(PlayerBehaviour pb, StateMachine fsm)
    {
        this.pb = pb;
        this.fsm = fsm;
    }
}

