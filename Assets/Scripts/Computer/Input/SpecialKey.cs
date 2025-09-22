using UnityEngine;

public class SpecialKey : InteractableKey
{
    enum specialKey
    {
        backspace,
        enter,
        capsLock,
        escape,
        leftClick,
        rightClick
    }
    [SerializeField] specialKey keyType;


    protected override void OnPress()
    {
        switch (keyType)
        {
            case specialKey.backspace:
                inputManager.BackSpace();
                break;
            case specialKey.enter:
                inputManager.Enter();
                break;
            case specialKey.capsLock:
                inputManager.CapsLock();
                break;
            case specialKey.escape:
                inputManager.Escape();
                break;
            case specialKey.leftClick:
                inputManager.LeftClick();
                break;
            case specialKey.rightClick:
                inputManager.RightClick();
                break;
        }
    }
}
