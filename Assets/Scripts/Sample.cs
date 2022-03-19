using ObjectPool;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private EventSystem eventSystem;
    private SpriteRenderer spriteRenderer;
    private ColorChange_Circulation change;

    private void Awake()
    {
        eventSystem = Service.Get<EventSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        change = new ColorChange_Circulation();
        change.Initialize(spriteRenderer.color, spriteRenderer.color.ResetAlpha(0f), 2f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            change.Paused = !change.Paused;
    }

    private void FixedUpdate()
    {
        spriteRenderer.color = change.Current;
    }
}
