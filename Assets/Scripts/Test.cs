using Character;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void FixedUpdate()
    {
        if(InputRecorder.GetButtonDown("Jump",EInputDuration.FromFixedUpdate))
        {
            Debug.Log(Time.time);
        }
    }
}
