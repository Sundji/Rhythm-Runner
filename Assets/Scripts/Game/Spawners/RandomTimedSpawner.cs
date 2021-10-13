using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class RandomTimedSpawner : BaseSpawner
    {
        [SerializeField] private List<MoveableObject> _objectsToSpawn = new List<MoveableObject>();
        [SerializeField] private float _spawnInterval = 3;

        private float _timer;

        protected override void Spawn()
        {
            int index = new Vector2Int(0, _objectsToSpawn.Count).GetRandomNumber();
            MoveableObject spawnedObject = Instantiate(_objectsToSpawn[index], _transform);
            spawnedObject.InitializeMoving(_transform.position);
        }

        private void Update()
        {
            if (_isSpawning)
            {
                _timer += Time.deltaTime;
                if (_timer > _spawnInterval)
                {
                    _timer = 0;
                    Spawn();
                }
            }
        }
    }
}