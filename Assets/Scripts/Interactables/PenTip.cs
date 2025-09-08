using UnityEngine;

public class PenTip : MonoBehaviour
{
    [SerializeField] Transform drawPoint;
    [SerializeField] float rayLength = 0.5f;
    [SerializeField] Color drawColor;
    [SerializeField] int brushSize;
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<DrawingBoard>(out DrawingBoard drawBoard))
        {
            Ray ray = new Ray(drawPoint.position, transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                drawBoard.DrawAtPosition(hit.textureCoord, drawColor, brushSize);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(drawPoint.position, transform.up * rayLength);
    }
}
