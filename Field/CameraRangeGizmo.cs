using UnityEngine;

namespace Field
{
    public class CameraRangeGizmo : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _boxCollider2D;
    
        void OnDrawGizmos() {
            Gizmos.color = new Color (0, 1, 0, 1f);
            Gizmos.DrawWireCube(_boxCollider2D.offset, _boxCollider2D.size);
        }
    }
}
