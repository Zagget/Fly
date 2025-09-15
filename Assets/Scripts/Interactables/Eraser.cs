using UnityEngine;

public class Eraser : MonoBehaviour
{
    [SerializeField] Transform[] erasePoints;
    [SerializeField] float rayLength = 0.1f;
    [SerializeField] Color eraseColor = Color.white;
    [Range(0, 50)][SerializeField] int eraseSizePerPoint;

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<DrawingBoard>(out DrawingBoard drawBoard))
        {
            foreach (Transform point in erasePoints)
            {
                Ray ray = new Ray(point.position, -transform.up);
                if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
                {
                    drawBoard.DrawAtPosition(hit.textureCoord, drawBoard.BoardColor, eraseSizePerPoint);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (erasePoints.Length > 0)
        {
            foreach (Transform point in erasePoints)
                {
                    Gizmos.DrawRay(point.position, -transform.up * rayLength);
                    Gizmos.DrawSphere(point.position + (-transform.up * rayLength), eraseSizePerPoint * 0.03f);
                }
        }
    }
}
