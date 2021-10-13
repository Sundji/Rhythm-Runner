using System;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public abstract class BasePoints : MonoBehaviour
    {
        public static Action<int> OnPointsCollected;

        protected virtual void CollectPoints()
        {
            OnPointsCollected?.Invoke(GetPoints());
        }

        protected virtual int GetPoints()
        {
            return Constants.DEFAULT_POINTS;
        }
    }
}