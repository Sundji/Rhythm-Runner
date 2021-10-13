using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class DestructibleObstacle : Obstacle
    {
        [Header("Trigger Values")]
        [SerializeField] private List<Trigger> _triggers = new List<Trigger>();

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.OBSTACLE_COLLISION_TAG) && !_hasCollided)
            {
                _hasCollided = true;
                StartCoroutine(CheckIfDestroyedBeforeHit());
            }
        }

        protected override void CollectPoints()
        {
            bool isTriggered = true;
            foreach (Trigger trigger in _triggers)
            {
                if (!trigger.IsTriggered)
                {
                    isTriggered = false;
                    break;
                }
            }
            if (isTriggered)
            {
                OnPointsCollected.Invoke(GetPoints());
            }
        }

        protected override int GetPoints()
        {
            return Constants.DESTRUCTIBLE_OBSTACLE_POINTS;
        }

        private IEnumerator CheckIfDestroyedBeforeHit()
        {
            yield return new WaitForEndOfFrame();

            bool isTriggered = true;
            foreach (Trigger trigger in _triggers)
            {
                if (!trigger.IsTriggered)
                {
                    isTriggered = false;
                }
                trigger.Deactivate();
            }

            if (!isTriggered)
            {
                OnPlayerHit?.Invoke(_damageAmount);
            }
            yield return null;
        }
    }
}