using System;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class Coin : BasePoints
    {
        [Header("Collision")]
        [SerializeField] private VRControllerType _collisionController = VRControllerType.LEFT_CONTROLLER;

        private HapticImpulseSender _hapticImpulseSender = new HapticImpulseSender();

        protected override int GetPoints()
        {
            return Constants.COIN_POINTS;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GetCollisionTag()))
            {
                _hapticImpulseSender.SendHapticImpulse(_collisionController, 
                    Constants.POSITIVE_HAPTICS_FEEDBACK_AMPLITUDE, 
                    Constants.POSITIVE_HAPTICS_FEEDBACK_DURATION);
                CollectPoints();
                Destroy(gameObject);
            }
        }

        private string GetCollisionTag()
        {
            if (_collisionController == VRControllerType.LEFT_CONTROLLER)
            {
                return Constants.CONTROLLER_LEFT_TAG;
            }
            return Constants.CONTROLLER_RIGHT_TAG;
        }
    }
}