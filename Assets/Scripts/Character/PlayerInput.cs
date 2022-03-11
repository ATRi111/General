using UnityEngine;

public class PlayerInput : CharacterController
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
