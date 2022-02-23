public class GameManager : Service
{
    private EventSystem eventSystem;

    protected override void BeforeRegister()
    {
        eService = EService.GameManager;
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>(EService.EventSystem);
    }
}
