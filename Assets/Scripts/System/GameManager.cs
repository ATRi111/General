public class GameManager : Service
{
    private EventSystem eventSystem;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>();
    }
}
