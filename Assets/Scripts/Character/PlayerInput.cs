using UnityEngine;

public class PlayerInput : CharacterController
{
    private InputManager inputManager;

    protected override void Awake()
    {
        base.Awake();
        inputManager = Service.Get<InputManager>();
    }
}
