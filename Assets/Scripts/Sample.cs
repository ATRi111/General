using ObjectPool;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private class IntChange : MyTimer<float>
    {
        public IntChange(float origin, float target, float duration) : base(origin, target, duration) { }
        public override float Current => Origin + (Target - Origin) * (Timer / Duration);
    }

    private EventSystem eventSystem;
    public float a;

    private void Awake()
    {
        eventSystem = ServiceLocator.GetService<EventSystem>();
    }

    private void Update()
    {

    }
}
