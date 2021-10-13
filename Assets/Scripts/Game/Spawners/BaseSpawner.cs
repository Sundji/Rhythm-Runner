using UnityEngine;

namespace Divit.RhythmRunner
{
    public abstract class BaseSpawner : MonoBehaviour
    {
        protected Transform _transform;

        protected bool _isSpawning = true;

        public virtual void Activate()
        {
            _isSpawning = true;
        }

        public virtual void Deactivate()
        {
            _isSpawning = false;
        }

        private void Awake()
        {
            _transform = transform;
        }

        protected abstract void Spawn();
    }
}