using UnityEngine;

public class SymbolKey : InteractableKey
{

    [SerializeField] char symbol;

    protected override void OnPress()
    {
        inputManager.OnPress(symbol);
    }
}
