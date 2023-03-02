using Character;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void FixedUpdate()
    {
        if(InputRecoder.GetButtonDown("Jump",EInputDuration.FromFixedUpdate))
        {
            Debug.Log(Time.time);
        }
    }
}
