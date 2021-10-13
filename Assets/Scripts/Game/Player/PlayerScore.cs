using System;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class PlayerScore : MonoBehaviour
    {
        public static Action OnScoreChanged;

        public int Score
        {
            get;
            private set;
        }

        public void AddToScore(int points)
        {
            Score += points;
            OnScoreChanged?.Invoke();
        }

        public void ResetPlayerScore()
        {
            Score = 0;
        }
    }
}