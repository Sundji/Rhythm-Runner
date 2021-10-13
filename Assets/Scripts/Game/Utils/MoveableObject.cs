using UnityEngine;

namespace Divit.RhythmRunner
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveableObject : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Vector3 _movementDirection = Vector3.back;

        [Header("Position Offset")]
        [SerializeField] private Vector3 _positionOffset = Vector3.zero;

        public void InitializeMoving(Vector3 position)
        {
            transform.position = position + _positionOffset;
            GetComponent<Rigidbody>().velocity = _movementDirection * Constants.MOVEABLE_OBJECT_SPEED;
        }
    }
}