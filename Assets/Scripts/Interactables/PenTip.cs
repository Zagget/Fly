using UnityEngine;

public class PenTip : MonoBehaviour
{
    [SerializeField] Transform drawPoint;
    [SerializeField] float rayLength = 0.1f;
    [SerializeField] Color drawColor;
    [Range(0, 20)][SerializeField] int brushSize;
    private float drawCooldown = 0.05f;
    private float lastDrawTime = 0f;

    void OnCollisionStay(Collision other)
    {
        if (Time.time - lastDrawTime < drawCooldown) return;

        if (other.gameObject.TryGetComponent<DrawingBoard>(out DrawingBoard drawBoard))
        {
            Ray ray = new Ray(drawPoint.position, -other.contacts[0].normal);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                drawBoard.DrawAtPosition(hit.textureCoord, drawColor, brushSize);
                lastDrawTime = Time.time;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (drawPoint)
        {
            Gizmos.DrawRay(drawPoint.position, transform.up * rayLength);
            Gizmos.color = drawColor;
            Gizmos.DrawSphere(drawPoint.position + transform.up * rayLength, brushSize * 0.03f);
        }
    }
}
