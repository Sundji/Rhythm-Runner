using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class TutorialSpawner : MonoBehaviour
    {
        [SerializeField] private MoveableObject _obstacleForCollision = null;
        [SerializeField] private List<MoveableObject> _obstaclePrefabs = new List<MoveableObject>();
        [SerializeField] private List<MoveableObject> _destructibleObstaclePrefabs = new List<MoveableObject>();
        [SerializeField] private List<MoveableObject> _coinPrefabs = new List<MoveableObject>();

        [SerializeField] private float _spawnInterval = 2;

        private Transform _transform;

        public void SpawnNext(TutorialPartType tutorialPartType)
        {
            StopAllCoroutines();
            switch (tutorialPartType)
            {
                case TutorialPartType.COIN:
                    StartCoroutine(SpawnObjects(_coinPrefabs));
                    break;
                case TutorialPartType.DESTRUCTIBLE_OBSTACLE:
                    StartCoroutine(SpawnObjects(_destructibleObstaclePrefabs));
                    break;
                case TutorialPartType.OBSTACLE:
                    StartCoroutine(SpawnObjects(_obstaclePrefabs));
                    break;
                case TutorialPartType.OBSTACLE_COLLISION:
                    SpawnObject(_obstacleForCollision);
                    break;
            }
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void SpawnObject(MoveableObject objectPrefab)
        {
            MoveableObject moveableObject = Instantiate(objectPrefab);
            moveableObject.InitializeMoving(_transform.position);
        }

        private IEnumerator SpawnObjects(List<MoveableObject> objectPrefabs)
        {
            foreach (MoveableObject moveableObjectPrefab in objectPrefabs)
            {
                SpawnObject(moveableObjectPrefab);
                yield return new WaitForSeconds(_spawnInterval);
            }
            yield return null;
        }
    }
}