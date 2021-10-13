using UnityEngine;

namespace Divit.RhythmRunner
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] private VRControllerType _activatorController = VRControllerType.LEFT_CONTROLLER;

        private HapticImpulseSender _hapticImpulseSender = new HapticImpulseSender();

        private bool _isActive = true;

        public bool IsTriggered
        {
            get;
            private set;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive)
            {
                if (other.CompareTag(GetCollisionTag()))
                {
                    _hapticImpulseSender.SendHapticImpulse(_activatorController, 
                        Constants.POSITIVE_HAPTICS_FEEDBACK_AMPLITUDE, 
                        Constants.POSITIVE_HAPTICS_FEEDBACK_DURATION);
                    IsTriggered = true;
                }
            }
        }

        private string GetCollisionTag()
        {
            if (_activatorController == VRControllerType.LEFT_CONTROLLER)
            {
                return Constants.CONTROLLER_LEFT_TAG;
            }
            return Constants.CONTROLLER_RIGHT_TAG;
        }
    }
}