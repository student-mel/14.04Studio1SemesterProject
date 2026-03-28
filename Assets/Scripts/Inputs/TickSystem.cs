
public abstract class TickSystem
{
    float interval;
    float timer;

    protected TickSystem(float _interval)
    {
        interval = _interval;
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
        if(timer >= interval)
        {
            timer -= interval;
            Tick();
        }
    }
    protected abstract void Tick();
}
