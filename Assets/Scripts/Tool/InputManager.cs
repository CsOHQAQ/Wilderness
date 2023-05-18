using QxFramework.Core;
using QxFramework.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputManager : Singleton<InputManager>, ISystemModule
{
    private Dictionary<InputEnum, Tuple<KeyCode, XboxButton>> defaultButtonConfig = new Dictionary<InputEnum, Tuple<KeyCode, XboxButton>>
    {
        { InputEnum.Up, new Tuple<KeyCode, XboxButton>(KeyCode.W, XboxButton.Up) },
        { InputEnum.Down, new Tuple<KeyCode, XboxButton>(KeyCode.S, XboxButton.Down) },
        { InputEnum.Left, new Tuple<KeyCode, XboxButton>(KeyCode.A, XboxButton.Left) },
        { InputEnum.Right, new Tuple<KeyCode, XboxButton>(KeyCode.D, XboxButton.Right) },
        { InputEnum.Interact, new Tuple<KeyCode, XboxButton>(KeyCode.Space, XboxButton.Y) },
        {InputEnum.SwitchUp,new Tuple<KeyCode, XboxButton>(KeyCode.Q,XboxButton.Right) },
        { InputEnum.Menu, new Tuple<KeyCode, XboxButton>(KeyCode.Escape, XboxButton.Menu) },
        { InputEnum.Bag, new Tuple<KeyCode, XboxButton>(KeyCode.E, XboxButton.Menu) },
        { InputEnum.Quit, new Tuple<KeyCode, XboxButton>(KeyCode.Escape, XboxButton.Start) },
    };

    private Dictionary<InputEnum, Tuple<KeyCode, XboxButton>> buttonConfig;
    private PlayerIndex playerIndex = PlayerIndex.One;

    public Vector2 Direction
    {
        get
        {
            Vector2 dir = Vector2.zero;
            if (XInput.GetButton(playerIndex, buttonConfig[InputEnum.Up].Item2))
            {
                dir += Vector2.up;
            }
            if (XInput.GetButton(playerIndex, buttonConfig[InputEnum.Down].Item2))
            {
                dir += Vector2.down;
            }
            if (XInput.GetButton(playerIndex, buttonConfig[InputEnum.Left].Item2))
            {
                dir += Vector2.left;
            }
            if (XInput.GetButton(playerIndex, buttonConfig[InputEnum.Right].Item2))
            {
                dir += Vector2.right;
            }
            if (Input.GetKey(buttonConfig[InputEnum.Up].Item1))
            {
                dir += Vector2.up;
            }
            if (Input.GetKey(buttonConfig[InputEnum.Down].Item1))
            {
                dir += Vector2.down;
            }
            if (Input.GetKey(buttonConfig[InputEnum.Left].Item1))
            {
                dir += Vector2.left;
            }
            if (Input.GetKey(buttonConfig[InputEnum.Right].Item1))
            {
                dir += Vector2.right;
            }
            dir += new Vector2(XInput.GetAxis(playerIndex, XboxAxis.LX), XInput.GetAxis(playerIndex, XboxAxis.LY));

            return dir.normalized;
        }
    }

    public Vector2 Aim
    {
        get
        {
            return Vector2.zero;
        }
    }

    public Vector2 MousePos
    {
        get
        {
            return Input.mousePosition;
        }
    }
    public Vector2 MouseWorldPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    public override void Initialize()
    {
        base.Initialize();
        //TODO 如果读取不到设定就用初始键位
        ReadButtonConfig();
    }

    public void Update(float deltaTime)
    {

    }

    public void FixedUpdate(float deltaTime)
    {

    }

    public void Dispose()
    {

    }

    private void ReadButtonConfig()
    {
        buttonConfig = defaultButtonConfig;
    }

    public bool GetButtonDown(InputEnum buttonName)
    {
        if (buttonConfig.ContainsKey(buttonName))
        {
            return Input.GetKeyDown(buttonConfig[buttonName].Item1) || XInput.GetButtonDown(playerIndex, buttonConfig[buttonName].Item2);
        }
        Debug.LogError("未找到\"" + buttonName + "\"对应的按键");
        return false;
    }

    public bool GetButton(InputEnum buttonName)
    {
        if (buttonConfig.ContainsKey(buttonName))
        {
            return Input.GetKey(buttonConfig[buttonName].Item1) || XInput.GetButton(playerIndex, buttonConfig[buttonName].Item2);
        }
        Debug.LogError("未找到\"" + buttonName + "\"对应的按键");
        return false;
    }

    public bool GetButtonUp(InputEnum buttonName)
    {
        if (buttonConfig.ContainsKey(buttonName))
        {
            return Input.GetKeyUp(buttonConfig[buttonName].Item1) || XInput.GetButtonUp(playerIndex, buttonConfig[buttonName].Item2);
        }
        Debug.LogError("未找到\"" + buttonName + "\"对应的按键");
        return false;
    }
}

public enum InputEnum
{
    Unknown,
    Up, 
    Down, 
    Left,
    Right,
    Run,
    Duck,
    Command,
    Jump,
    Reload,
    Interact,
    Fire,
    Aim,
    Heal,
    SwitchUp,
    SwitchDown,
    Menu,
    Bag,
    Quit,
}


