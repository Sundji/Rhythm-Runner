using UnityEngine;

namespace Divit.RhythmRunner
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform = null;

        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
#if !UNITY_EDITOR
            _transform.position = _targetTransform.position;
#endif
        }
    }
}