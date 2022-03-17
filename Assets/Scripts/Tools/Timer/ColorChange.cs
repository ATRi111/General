using UnityEngine;

public class ColorChange : MyTimer<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Percent);
}

public class ColorChange_Circulation : Circulation<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Percent);
}

public class ColorChange_Repeatation : Repeataion<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Percent);
}
