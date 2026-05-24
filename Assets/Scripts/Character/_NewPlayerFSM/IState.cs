public interface IState
{
    void Enter(object data = null);
    void Update();
    void FixedUpdate();
    void Exit();
}