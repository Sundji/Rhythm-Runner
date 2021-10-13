using System;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class Obstacle : BasePoints
    {
        [Header("Damage Amount")]
        [SerializeField] protected int _damageAmount = 10;

        public static Action<int> OnPlayerHit;

        protected bool _hasCollided;

        protected void OnDestroy()
        {
            CollectPoints();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.OBSTACLE_COLLISION_TAG) && !_hasCollided)
            {
                _hasCollided = true;
                OnPlayerHit?.Invoke(_damageAmount);
            }
        }

        protected override void CollectPoints()
        {
            if (!_hasCollided)
            {
                OnPointsCollected?.Invoke(GetPoints());
            }
        }

        protected override int GetPoints()
        {
            return Constants.OBSTACLE_POINTS;
        }
    }
}