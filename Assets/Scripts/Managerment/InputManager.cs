using NaughtyAttributes;
using ToolBox.Serialization;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public PlayerControls playerControls;
    public UIControls uiControls;
    [Button]
    public void DeleteAllSave()
    {
        DataSerializer.DeleteAll();
    }

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        uiControls = new UIControls();
    }

    public void PauseInputSystem(bool status)
    {
        if (!status)
        {
            playerControls.Enable();
            uiControls.Enable();
        }
        else
        {
            playerControls.Disable();
            uiControls.Disable();
        }
    }
}
