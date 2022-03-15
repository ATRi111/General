public class GameManager : Service
{
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = ServiceLocator.GetService<EventSystem>();
        DontDestroyOnLoad(gameObject);
    }
}
