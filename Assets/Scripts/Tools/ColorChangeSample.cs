using UnityEngine;

public class ColorChangeSample : MonoBehaviour
{
    private class ColorChange_Circulation : Circulation<Color>
    {
        public override Color Current => Color.Lerp(Origin, Target, Timer / Duration);
        public ColorChange_Circulation(Color origin, Color target, float duration) : base(origin, target, duration) { }
    }

    public class ColorChange_Repeatation : Repeataion<Color>
    {
        public override Color Current => Color.Lerp(Origin, Target, Timer / Duration);
        public ColorChange_Repeatation(Color origin, Color target, float duration) : base(origin, target, duration) { }
    }

    private ColorChange_Repeatation change;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        change = new ColorChange_Repeatation(spriteRenderer.color, spriteRenderer.color.TransparentColor(), 1f);
    }

    private void FixedUpdate()
    {
        change.OnFixedUpdate();
        spriteRenderer.color = change.Current;
    }

}
