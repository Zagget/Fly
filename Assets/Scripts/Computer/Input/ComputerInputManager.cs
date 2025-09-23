using UnityEngine;

public class ComputerInputManager : MonoBehaviour
{
    [SerializeField] VirtualComputerMouse mouse;

    ComputerElement selectedElement;
    bool capsLock = false;

    public void OnPress(char key)
    {
        if (selectedElement != null && selectedElement is IOnPress onPress) 
        {
            if (capsLock) key = char.ToUpper(key);
            onPress.OnPress(key); 
        }
    }
    public void BackSpace() 
    {
        if (selectedElement != null && selectedElement is IBackSpace backSpace) backSpace.BackSpace();
    }
    public void Enter()
    { 
        if (selectedElement != null && selectedElement is IEnter enter) enter.Enter();
    }

    public void Escape()
    {
        if (selectedElement != null) NewElement(selectedElement.ParentElement);
    }

    private void NewElement(ComputerElement element)
    {
        if (selectedElement != null) selectedElement.Deselected();
        selectedElement = element;
        if (selectedElement != null) selectedElement.InternalSelected();
    }

    public void LeftClick() 
    {
        ComputerElement element = mouse.Click();
        NewElement(element);
        if (selectedElement != null && selectedElement is ILeftClick leftClick) leftClick.LeftClick();
    }
    public void RightClick() 
    {
        ComputerElement element = mouse.Click();
        if (element != null && element is IRightClick rightClick) rightClick.RightClick();
    }
    public void MoveMouse(Vector3 deltaMovement) { mouse.Move(deltaMovement); }

    public void CapsLock() { capsLock = !capsLock; }
}
    