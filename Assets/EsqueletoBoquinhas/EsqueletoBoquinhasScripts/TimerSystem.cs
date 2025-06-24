public class TimerSystem : StaticInstance<TimerSystem>
{
    public bool isTimerEnabled = true;

    public void EnableTimer()
    {
        isTimerEnabled = true;
    }

    public void DisableTimer()
    {
        isTimerEnabled = false;
    }
}