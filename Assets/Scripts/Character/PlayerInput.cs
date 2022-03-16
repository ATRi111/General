using UnityEngine;

public class PlayerInput : CharacterController
{
    private InputManager inputManager;
    private Camera main;

    protected override void Awake()
    {
        base.Awake();
        main = Camera.main;
        inputManager = ServiceLocator.GetService<InputManager>();
    }
}
