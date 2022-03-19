public class GameManager : Service
{
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = Get<EventSystem>();
        DontDestroyOnLoad(gameObject);
    }
}
