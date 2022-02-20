public class GameManager : Service
{
    private EventSystem eventSystem;

    protected override void Awake()
    {
        eService = EService.GameManager;
        base.Awake();
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>(EService.EventSystem);
    }
}
