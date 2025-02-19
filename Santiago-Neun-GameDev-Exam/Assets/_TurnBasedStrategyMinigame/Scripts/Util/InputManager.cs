using UnityEngine;
using NF.Main.Core;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{

    //InputSystem_Actions won't update so used own unit input actions
    private InputSystem_Actions _actions;
    private UnitInputActions _unitInputActions;

    private void Awake()
    {
        Instance = this;
        _actions = new InputSystem_Actions();
        _unitInputActions = new UnitInputActions();
        _actions.Player.Enable();
        _unitInputActions.Player.Enable();
    }


    public Vector2 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public bool IsMouseButtonDown()
    {
        return _unitInputActions.Player.Click.WasPressedThisFrame();
    }

    public Vector2 GetCameraMoveVector()
    {
        return _actions.Player.Move.ReadValue<Vector2>();
    }

    public float GetCameraRotateAmount()
    {
        return _unitInputActions.Player.CameraRotate.ReadValue<float>();
    }

    public float GetCameraZoomAmount()
    {
        return _unitInputActions.Player.CameraZoom.ReadValue<float>();
    }


}

