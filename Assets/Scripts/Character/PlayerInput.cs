using UnityEngine;

public class PlayerInput : CharacterController
{
    private Camera main;

    protected override void Awake()
    {
        base.Awake();
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
