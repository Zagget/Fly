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

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
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

        var renderer = pointerCube.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;

        SetVisible(false);
        boxCollider.enabled = false;
    }

    public void SetVisible(bool visible)
    {
        pointerCube.SetActive(visible);
        boxCollider.enabled = visible;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((uiLayer.value & (1 << other.transform.gameObject.layer)) == 0)
            return;

        VRButton button = other.GetComponent<VRButton>();
        if (button != null)
        {

            Debug.Log("MENU Hover true" + other.name);
            button.Hover(true);
            currentButton = button;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((uiLayer.value & (1 << other.transform.gameObject.layer)) == 0)
            return;

        VRButton button = other.GetComponent<VRButton>();
        if (button != null)
        {
            Debug.Log("MENU Hover false" + other.name);
            button.Hover(false);
            currentButton = null;
        }
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.started && currentButton != null)
        {
            Debug.Log("MENU: Pressed on " + currentButton.name);
            currentButton.Press();
        }
    }
}