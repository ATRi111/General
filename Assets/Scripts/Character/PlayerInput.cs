using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera main;

    private void Awake()
    {
        main = Camera.main;
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {

    }
}
