
public static class GameClock
{
    public static int Frame {  get; private set; }

    public static void Tick()
    {
        Frame++;
    }
}
