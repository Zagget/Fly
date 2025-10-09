using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPointer : MonoBehaviour
{
    public LayerMask uiLayer;

    public BoxCollider boxCollider;
    public Rigidbody rb;

    private GameObject pointerCube;
    public Color color = new Color(1f, 0f, 0f, 0.25f);
    private VRButton currentButton;

    private VRButton[] allButtons;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        allButtons = Object.FindObjectsByType<VRButton>(FindObjectsSortMode.None);
    }

    void Start()
    {
        rb.isKinematic = true;
        rb.useGravity = false;

        pointerCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Destroy(pointerCube.GetComponent<Collider>());

        pointerCube.transform.SetParent(transform, false);

        pointerCube.transform.localPosition = boxCollider.center;
        pointerCube.transform.localScale = boxCollider.size;

        pointerCube.layer = LayerMask.NameToLayer("Overlay UI");

        var renderer = pointerCube.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;

        SetVisible(false);
        boxCollider.enabled = false;

        SetVisible(true);
    }

    public void SetVisible(bool visible)
    {
        if (pointerCube == null) return; //controllers are not connected so start has not been executed yet.

        pointerCube.SetActive(visible);
        boxCollider.enabled = visible;

        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].Hover(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((uiLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        VRButton button = other.GetComponent<VRButton>();
        if (button != null)
        {
            if (currentButton != null && currentButton != button)
                currentButton.Hover(false);

            Debug.Log("MENU Hover true " + other.name);
            button.Hover(true);
            currentButton = button;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((uiLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        VRButton button = other.GetComponent<VRButton>();
        if (button != null && button == currentButton)
        {
            Debug.Log("MENU Hover false" + other.name);
            button.Hover(false);
            currentButton = null;
        }
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        Debug.Log($"MENU: ONPRESS currentbutton active: {currentButton != null}");
        if (context.started && currentButton != null)
        {
            Debug.Log("MENU: Pressed on " + currentButton.name);
            currentButton.Press();
        }
    }
}