using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Divit.RhythmRunner
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("XR Variables")]
        [SerializeField] private XRRig _xrRig = null;
        [SerializeField] private XRController _xrLeftController = null;
        [SerializeField] private XRController _xrRightController = null;

        [Header("Collision Variables")]
        [SerializeField] private CapsuleCollider _playerBodyCollider = null;
        [SerializeField] private SphereCollider _playerHeadCollider = null;
        [SerializeField] private float _collisionRadius = 0.2f;

        [Header("Particle Effects")]
        [SerializeField] private GameObject _particleEffectLeftController = null;
        [SerializeField] private GameObject _particleEffectRightController = null;

        private float _playerHeight;

        public void ResetPlayerMovement()
        {
#if !UNITY_EDITOR
            StartCoroutine(Initialize());
#endif
#if UNITY_EDITOR
            _playerHeight = _xrRig.cameraYOffset;
#endif  
        }

        public void SendHapticImpulse(VRControllerType vrControllerType, float amplitude, float duration)
        {
            if (vrControllerType == VRControllerType.BOTH || vrControllerType == VRControllerType.LEFT_CONTROLLER)
            {
                SendHapticImpulseToController(_xrLeftController, amplitude, duration);
                Instantiate(_particleEffectLeftController, _xrLeftController.transform);
            }
            if (vrControllerType == VRControllerType.BOTH || vrControllerType == VRControllerType.RIGHT_CONTROLLER)
            {
                SendHapticImpulseToController(_xrRightController, amplitude, duration);
                Instantiate(_particleEffectRightController, _xrRightController.transform);
            }
        }

        private void Awake()
        {
#if !UNITY_EDITOR
            StartCoroutine(Initialize());
#endif
#if UNITY_EDITOR
            _playerHeight = _xrRig.cameraYOffset;
#endif
        }

        private IEnumerator Initialize()
        {
            while (_xrRig.cameraInRigSpaceHeight == 0)
            {
                yield return null;
            }

            _playerHeight = _xrRig.cameraInRigSpaceHeight;

            Vector2 playerBodyColliderCenter = _playerBodyCollider.center;
            playerBodyColliderCenter.y = -(_playerHeight / 2 + _collisionRadius);
            _playerBodyCollider.center = playerBodyColliderCenter;
            _playerBodyCollider.height = _playerHeight - _collisionRadius * 2;
            _playerBodyCollider.radius = _collisionRadius;

            Vector3 playerHeadColliderCenter = _playerHeadCollider.center;
            playerHeadColliderCenter.y = -_collisionRadius;
            _playerHeadCollider.center = playerHeadColliderCenter;
            _playerHeadCollider.radius = _collisionRadius;

            yield return null;
        }

        private void SendHapticImpulseToController(XRController controller, float amplitude, float duration)
        {
            if (controller.inputDevice.TryGetHapticCapabilities(out UnityEngine.XR.HapticCapabilities hapticCapabilities))
            {
                if (hapticCapabilities.supportsImpulse)
                {
                    controller.SendHapticImpulse(amplitude, duration);
                }
            }
        }
    }
}
