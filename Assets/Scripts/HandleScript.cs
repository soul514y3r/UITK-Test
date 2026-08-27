using UnityEngine;

public class HandleScript : MonoBehaviour
{
    public Vector3 Size;
    public Vector3 Lastpos;

    public void OnDrawGizmos()
    {
    Matrix4x4 oldMatrix = Gizmos.matrix;

    Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(Lastpos, Size);

    Gizmos.matrix = oldMatrix;
    }
}
